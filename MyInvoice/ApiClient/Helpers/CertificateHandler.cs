using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
//using Microsoft.AspNetCore.Http;

namespace MyInvois.ApiClient.Helpers
{
    public static class CertificateHandler
    {
        public static (string certificateContent, string privateKeyContent) GetCertificateContents(string pfxPath, string password)
        {
            X509Certificate2 certificate = new(pfxPath, password, X509KeyStorageFlags.Exportable);
            return ExtractCertificateContents(certificate);
        }

        public static (string certificateContent, string privateKeyContent) GetCertificateContents(byte[] pfxBytes, string password)
        {
            X509Certificate2 certificate = new(pfxBytes, password, X509KeyStorageFlags.Exportable);
            return ExtractCertificateContents(certificate);
        }

        // public static async Task<(string certificateContent, string privateKeyContent)> GetCertificateContents(IFormFile pfxFile, string password)
        // {
        //     using var memoryStream = new MemoryStream();
        //     await pfxFile.CopyToAsync(memoryStream);
        //     return GetCertificateContents(memoryStream.ToArray(), password);
        // }
        private static (string certificateContent, string privateKeyContent) ExtractCertificateContents(X509Certificate2 certificate)
        {
            string certificateContent = Convert.ToBase64String(certificate.Export(X509ContentType.Cert));

            // Try to get RSA private key first
            RSA rsaKey = certificate.GetRSAPrivateKey();
            if (rsaKey != null)
            {
                byte[] rsaKeyBytes = rsaKey.ExportRSAPrivateKey();
                string privateKeyContent = Convert.ToBase64String(rsaKeyBytes);
                return (certificateContent, privateKeyContent);
            }

            // If not RSA, try ECDSA
            ECDsa ecdsaKey = certificate.GetECDsaPrivateKey();
            if (ecdsaKey != null)
            {
                byte[] ecdsaKeyBytes = ecdsaKey.ExportECPrivateKey();
                string privateKeyContent = Convert.ToBase64String(ecdsaKeyBytes);
                return (certificateContent, privateKeyContent);
            }

            throw new CryptographicException("Certificate does not contain a supported private key (RSA or ECDSA)");
        }
        public static string GetSerialNumber(X509Certificate2 certificate)
        {
            sbyte[] numArray = (from x in certificate.GetSerialNumber() select (sbyte)x).ToArray();
            System.Numerics.BigInteger integer = new((byte[])(Array)numArray);
            return integer.ToString();
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
                $"E={email}, " +
                $"SERIALNUMBER={brn}, " +
                $"CN={companyName}, " +
                $"OU={orgUnit}, " +
                $"OID.2.5.4.97={tin}, " +
                $"O={companyName}, " +
                $"C=MY"
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