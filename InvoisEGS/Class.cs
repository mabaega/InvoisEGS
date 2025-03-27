namespace InvoisEGS
{
    using System;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;

    public static class CertTest
    {
        public static void ExtractCertificateNative()
        {
            string pfxFilePath = @"C:\Users\Incredible One\source\repos\InvoisEGS\InvoisEGS\test-certificate.pfx";
            string pfxPassword = "12345678";

            try
            {
                Console.WriteLine("Starting native certificate extraction with different flags...");
                
                // Try with MachineKeySet and PersistKeySet flags
                using var certificate = new X509Certificate2(
                    pfxFilePath,
                    pfxPassword,
                    X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet
                );
                
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

                Console.WriteLine("\nCertificate:");
                Console.WriteLine(certificatePem);
                Console.WriteLine("\nPrivate Key:");
                Console.WriteLine(string.IsNullOrEmpty(privateKeyPem) ? "*** EMPTY ***" : privateKeyPem);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        private static string ConvertRSAParametersToPem(RSAParameters parameters)
        {
            // Create a new RSA instance in a non-Windows context
            using var rsa = RSA.Create();
            rsa.ImportParameters(parameters);
            
            try
            {
                // Try to export the key directly
                byte[] keyBytes = rsa.ExportPkcs8PrivateKey();
                return FormatPem(Convert.ToBase64String(keyBytes), "PRIVATE KEY");
            }
            catch
            {
                // If that fails, manually construct the ASN.1 structure
                using var ms = new MemoryStream();
                using var writer = new BinaryWriter(ms);
                
                // Write ASN.1 sequence header
                writer.Write((byte)0x30); // SEQUENCE
                
                // We'll come back and write the length
                long lengthPosition = ms.Position;
                writer.Write((byte)0x00); // Placeholder
                
                // Version
                writer.Write((byte)0x02); // INTEGER
                writer.Write((byte)0x01); // Length
                writer.Write((byte)0x00); // Value (version 0)
                
                // Write key parameters
                WriteAsn1Integer(writer, parameters.Modulus);
                WriteAsn1Integer(writer, parameters.Exponent);
                WriteAsn1Integer(writer, parameters.D);
                WriteAsn1Integer(writer, parameters.P);
                WriteAsn1Integer(writer, parameters.Q);
                WriteAsn1Integer(writer, parameters.DP);
                WriteAsn1Integer(writer, parameters.DQ);
                WriteAsn1Integer(writer, parameters.InverseQ);
                
                // Go back and write the length
                long endPosition = ms.Position;
                ms.Position = lengthPosition;
                writer.Write((byte)(endPosition - lengthPosition - 1));
                ms.Position = endPosition;
                
                return FormatPem(Convert.ToBase64String(ms.ToArray()), "RSA PRIVATE KEY");
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

       
        private static void WriteAsn1Integer(BinaryWriter writer, byte[] value)
        {
            if (value == null) return;
            
            writer.Write((byte)0x02); // INTEGER tag
            
            // Skip leading zeros
            int start = 0;
            while (start < value.Length && value[start] == 0) start++;
            
            // Ensure positive number
            bool addZero = start < value.Length && (value[start] & 0x80) != 0;
            int length = value.Length - start;
            
            writer.Write((byte)(length + (addZero ? 1 : 0)));
            
            if (addZero)
                writer.Write((byte)0x00);
            
            for (int i = start; i < value.Length; i++)
                writer.Write(value[i]);
        }

      

        public static (string certificateContent, string privateKeyContent) GetCertificateContents()
        {
            string pfxPath = @"C:\Users\Incredible One\source\repos\InvoisEGS\InvoisEGS\test-certificate.pfx";
            string password = "12345678";
            X509Certificate2 certificate = new(pfxPath, password, X509KeyStorageFlags.Exportable);
            return ExtractCertificateContents(certificate);
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
            
            Console.WriteLine("Bouncy Private Key:");
            Console.WriteLine(privateKeyContent);
            Console.WriteLine("\nBouncy Public Key:");
            Console.WriteLine(certificateContent);

            return (certificateContent, privateKeyContent);
        }

    }
}