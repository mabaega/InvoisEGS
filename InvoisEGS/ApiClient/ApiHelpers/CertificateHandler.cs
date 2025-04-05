using InvoisEGS.ApiClient.ApiModels;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace InvoisEGS.ApiClient.ApiHelpers
{
    public static class CertificateHandler
    {
        public static (string certificateContent, string privateKeyContent) GetCertificateContents(string pfxPath, string password)
        {
            X509Certificate2 certificate = new(pfxPath, password, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);
            return ExtractCertificateContents(certificate);
        }
        public static async Task<(string certificateContent, string privateKeyContent)> GetCertificateContents(IFormFile pfxFile, string password)
        {
            using MemoryStream memoryStream = new();
            await pfxFile.CopyToAsync(memoryStream);
            return GetCertificateContents(memoryStream.ToArray(), password);
        }
        public static (string certificateContent, string privateKeyContent) GetCertificateContents(byte[] pfxBytes, string password)
        {
            X509Certificate2 certificate = new(pfxBytes, password, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);
            return ExtractCertificateNative(certificate);
        }

        private static (string certificateContent, string privateKeyContent) ExtractCertificateContents(X509Certificate2 certificate)
        {
            string certificateContent = Convert.ToBase64String(certificate.Export(X509ContentType.Cert));

            // Export the private key using PKCS12 and BouncyCastle
            byte[] pfxData = certificate.Export(X509ContentType.Pfx, "");

            using MemoryStream ms = new(pfxData);
            Org.BouncyCastle.Pkcs.Pkcs12Store store = new(ms, new char[0]);
            string? alias = store.Aliases.Cast<string>().FirstOrDefault();

            if (string.IsNullOrEmpty(alias) || !store.IsKeyEntry(alias))
            {
                throw new InvalidOperationException("No private key found in certificate");
            }

            Org.BouncyCastle.Pkcs.AsymmetricKeyEntry key = store.GetKey(alias);
            using StringWriter stringWriter = new();
            Org.BouncyCastle.OpenSsl.PemWriter pemWriter = new(stringWriter);
            pemWriter.WriteObject(key.Key);
            string privateKeyContent = stringWriter.ToString();
            return (certificateContent, privateKeyContent);
        }

        public static (string certificateContent, string privateKeyContent) ExtractCertificateNative(X509Certificate2 certificate)
        {

            try
            {
               
                Console.WriteLine($"Certificate loaded successfully: {certificate.Subject}");

                // Get certificate in PEM format
                string certificatePem = FormatPem(
                    Convert.ToBase64String(certificate.Export(X509ContentType.Cert)),
                    "CERTIFICATE"
                );

                // Try to get private key
                string privateKeyPem = "";
                using (var privateKey = certificate.GetRSAPrivateKey())
                {
                    if (privateKey != null)
                    {
                        Console.WriteLine("RSA private key obtained from certificate");
                        
                        try
                        {
                            // Try to export directly as PKCS#1 first (preferred for document signing)
                            Console.WriteLine("Attempting direct PKCS#1 export...");
                            byte[] pkcs1Bytes = privateKey.ExportRSAPrivateKey();
                            privateKeyPem = FormatPem(
                                Convert.ToBase64String(pkcs1Bytes),
                                "RSA PRIVATE KEY"  // PKCS#1 format
                            );
                            Console.WriteLine("PKCS#1 export successful");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Direct PKCS#1 export failed: {ex.Message}");
                            
                            try
                            {
                                // Fall back to PKCS#8 if PKCS#1 fails
                                Console.WriteLine("Attempting PKCS#8 export...");
                                byte[] privateKeyBytes = privateKey.ExportPkcs8PrivateKey();
                                privateKeyPem = FormatPem(
                                    Convert.ToBase64String(privateKeyBytes),
                                    "PRIVATE KEY"  // PKCS#8 format
                                );
                                Console.WriteLine("PKCS#8 export successful");
                            }
                            catch (Exception pkcs8Ex)
                            {
                                Console.WriteLine($"PKCS#8 export failed: {pkcs8Ex.Message}");
                                
                                // Try with a temporary file approach
                                try
                                {
                                    Console.WriteLine("Attempting export via temporary PFX...");
                                    string tempPfxPath = Path.GetTempFileName();
                                    try
                                    {
                                        // Export to PFX with no password
                                        byte[] pfxData = certificate.Export(X509ContentType.Pfx, "");
                                        File.WriteAllBytes(tempPfxPath, pfxData);
                                        
                                        // Import with different flags
                                        using var tempCert = new X509Certificate2(
                                            tempPfxPath, 
                                            "", 
                                            X509KeyStorageFlags.Exportable | X509KeyStorageFlags.UserKeySet
                                        );
                                        
                                        using var tempPrivateKey = tempCert.GetRSAPrivateKey();
                                        if (tempPrivateKey != null)
                                        {
                                            byte[] keyBytes = tempPrivateKey.ExportRSAPrivateKey();
                                            privateKeyPem = FormatPem(
                                                Convert.ToBase64String(keyBytes),
                                                "RSA PRIVATE KEY"
                                            );
                                            Console.WriteLine("Export via temporary PFX successful");
                                        }
                                    }
                                    finally
                                    {
                                        if (File.Exists(tempPfxPath))
                                            File.Delete(tempPfxPath);
                                    }
                                }
                                catch (Exception tempEx)
                                {
                                    Console.WriteLine($"Temporary PFX approach failed: {tempEx.Message}");
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Failed to get RSA private key from certificate");
                    }
                }

                //Console.WriteLine("\nCertificate:");
                //Console.WriteLine(certificatePem);
                //Console.WriteLine("\nPrivate Key:");
                //Console.WriteLine(string.IsNullOrEmpty(privateKeyPem) ? "*** EMPTY ***" : privateKeyPem);

                return (certificatePem, privateKeyPem);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return (string.Empty,string.Empty);
            }
        }

        
        private static string FormatPem(string base64Content, string label)
        {
            // Split the base64 string into 64-character lines
            var lines = new List<string>();
            for (int i = 0; i < base64Content.Length; i += 64)
            {
                lines.Add(base64Content.Substring(i, Math.Min(64, base64Content.Length - i)));
            }
            
            return $"-----BEGIN {label}-----\n{string.Join("\n", lines)}\n-----END {label}-----";
        }

        public static string GetSerialNumber(X509Certificate2 certificate)
        {
            sbyte[] numArray = (from x in certificate.GetSerialNumber() select (sbyte)x).ToArray();
            System.Numerics.BigInteger integer = new((byte[])(Array)numArray);
            return integer.ToString();
        }

        public static CertificateInfo ValidateCertificate(X509Certificate2 cert, string expectedTIN)
        {
            var match = System.Text.RegularExpressions.Regex.Match(cert.Subject, @"OID\.2\.5\.4\.97=([^,]+)");
            string tin = match.Success ? match.Groups[1].Value : string.Empty;
            bool isTINValid = !string.IsNullOrEmpty(tin) && tin.Equals(expectedTIN, StringComparison.OrdinalIgnoreCase);

            return new CertificateInfo
            {
                Subject = cert.Subject,
                Issuer = cert.Issuer,
                ValidFrom = cert.NotBefore,
                ValidTo = cert.NotAfter,
                SerialNumber = cert.SerialNumber,
                TaxpayerTIN = tin,
                IsValid = DateTime.Now >= cert.NotBefore &&
                         DateTime.Now <= cert.NotAfter &&
                         cert.HasPrivateKey &&
                         isTINValid,
                Certificate = cert
            };
        }

        public static X509Certificate2 LoadCertificate(string pfxPath, string password)
        {
            return new X509Certificate2(pfxPath, password, X509KeyStorageFlags.Exportable);
        }

        public static X509Certificate2 CreateTestCertificate(
            string tin,
            string brn,
            string companyName,
            string email = "noemail@org.com.my",
            string orgUnit = "Test Unit eInvoice")
        {
            using RSA rsa = RSA.Create(2048);
            X500DistinguishedName distinguishedName = new(
                $"CN={companyName}, " +
                $"OU={orgUnit}, " +
                $"O={companyName}, " +
                $"C=MY, " +
                $"SERIALNUMBER={brn}, " +
                $"OID.2.5.4.97={tin}, " +
                $"E={email}"
            );

            CertificateRequest request = new(
                distinguishedName,
                rsa,
                HashAlgorithmName.SHA256,
                RSASignaturePadding.Pkcs1
            );

            // Add key usages
            request.CertificateExtensions.Add(
                new X509KeyUsageExtension(
                    X509KeyUsageFlags.NonRepudiation,
                    critical: true)
            );

            // Add enhanced key usage for both Email Protection and Document Signing
            request.CertificateExtensions.Add(
                new X509EnhancedKeyUsageExtension(
                    new OidCollection {
                            new Oid("1.3.6.1.5.5.7.3.4"),      // Email Protection
                            new Oid("1.3.6.1.4.1.311.10.3.12") // Document Signing
                    },
                    critical: false)
            );

            // Add Subject Key Identifier
            request.CertificateExtensions.Add(
                new X509SubjectKeyIdentifierExtension(request.PublicKey, false));

            // Add Certificate Policies
            var certificatePoliciesExtension = new X509Extension(
                new Oid("2.5.29.32"),
                new byte[] {
                    0x30, 0x4A, // SEQUENCE
                    0x30, 0x48, // SEQUENCE PolicyInformation
                        0x06, 0x09, // OBJECT IDENTIFIER
                        0x2B, 0x06, 0x01, 0x04, 0x01, 0x83, 0x8A, 0x45, 0x01, // 1.3.6.1.4.1.50501.1
                        0x30, 0x3B, // SEQUENCE PolicyQualifierInfo
                            0x30, 0x39, // SEQUENCE
                                0x06, 0x08, // OBJECT IDENTIFIER
                                0x2B, 0x06, 0x01, 0x05, 0x05, 0x07, 0x02, 0x01, // id-qt-cps
                                0x16, 0x2D, // IA5String
                                0x68, 0x74, 0x74, 0x70, 0x73, 0x3A, 0x2F, 0x2F, 
                                0x77, 0x77, 0x77, 0x2E, 0x70, 0x6F, 0x73, 0x64, 
                                0x69, 0x67, 0x69, 0x63, 0x65, 0x72, 0x74, 0x2E, 
                                0x63, 0x6F, 0x6D, 0x2E, 0x6D, 0x79, 0x2F, 0x72, 
                                0x65, 0x70, 0x6F, 0x73, 0x69, 0x74, 0x6F, 0x72, 
                                0x79, 0x2F, 0x63, 0x70, 0x73 // https://www.posdigicert.com.my/repository/cps
                },
                critical: false);
            request.CertificateExtensions.Add(certificatePoliciesExtension);

            // Add CRL Distribution Points
            request.CertificateExtensions.Add(
                new X509Extension(
                    new Oid("2.5.29.31"), // CRL Distribution Points OID
                    System.Text.Encoding.ASCII.GetBytes("http://trialcrl.posdigicert.com.my/TrialLHDNMV1.crl"),
                    false));

            // Create certificate valid for 3 months (like the sample)
            X509Certificate2 certificate = request.CreateSelfSigned(
                DateTimeOffset.Now,
                DateTimeOffset.Now.AddYears(1)
            );

            return new X509Certificate2(
                certificate.Export(X509ContentType.Pfx, "password"),
                "password",
                X509KeyStorageFlags.Exportable
            );
        }
        public static void SaveTestCertificate(
            string tin,
            string brn,
            string companyName,
            string outputPath,
            string password,
            string email = "noemail@org.com.my",
            string orgUnit = "Test Unit eInvoice")
        {
            X509Certificate2 cert = CreateTestCertificate(tin, brn, companyName, email, orgUnit);
            File.WriteAllBytes(outputPath, cert.Export(X509ContentType.Pfx, password));
        }
    }
}