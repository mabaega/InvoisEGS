using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace InvoisEGS.ApiClient.ApiHelpers
{
    public static class XmlJsonUtilities
    {
        public static string LoadEmbededResources(string resourceName)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();

            // Get all resource names for debugging
            string[] resourceNames = assembly.GetManifestResourceNames();
            if (!resourceNames.Contains(resourceName))
            {
                throw new FileNotFoundException(
                    $"Resource {resourceName} not found. Available resources: {string.Join(", ", resourceNames)}");
            }

            using Stream? stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                throw new InvalidOperationException($"Could not load resource: {resourceName}");
            }

            using StreamReader reader = new(stream);
            return reader.ReadToEnd();
        }
        
        /* public static string SignHash(byte[] hash, string privateKey)
        {
            try
            {
                if (string.IsNullOrEmpty(privateKey))
                {
                    throw new ArgumentException("Private key content is empty");
                }

                // Always compute SHA256 hash from the input bytes
                byte[] hashBytes;
                using (var sha256 = SHA256.Create())
                {
                    hashBytes = sha256.ComputeHash(hash);
                }

                // Create RSA instance and import the private key
                using var rsa = RSA.Create();
                rsa.ImportFromPem(privateKey);

                // Use RSAPKCS1SignatureFormatter as per official sample
                RSAPKCS1SignatureFormatter rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
                rsaFormatter.SetHashAlgorithm(nameof(SHA256));
                byte[] signature = rsaFormatter.CreateSignature(hashBytes);

                return Convert.ToBase64String(signature);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error details: {ex.Message}");
                throw new CryptographicException("Failed to sign hash with private key", ex);
            }
        } */
        public static string SignHash(byte[] hash, string privateKey)
        {
            try
            {
                // Don't compute hash again, PowerShell uses the hash directly
                using var rsa = RSA.Create();
                rsa.ImportFromPem(privateKey);

                // Match PowerShell implementation
                RSAPKCS1SignatureFormatter rsaFormatter = new(rsa);
                rsaFormatter.SetHashAlgorithm("SHA256");
                byte[] signature = rsaFormatter.CreateSignature(hash);

                return Convert.ToBase64String(signature);
            }
            catch (Exception ex)
            {
                throw new CryptographicException("Failed to sign hash with private key", ex);
            }
        }
        public static string GetCertificateSerialNumberAsDecimal(X509Certificate2 cert)
        {
            string serialHex = cert.SerialNumber;
            System.Numerics.BigInteger serialNumber = System.Numerics.BigInteger.Parse(serialHex, System.Globalization.NumberStyles.HexNumber);
            return serialNumber.ToString();
        }

        public static string ComputeHash(byte[] data)
        {
            using var sha256 = SHA256.Create();
            byte[] hash = sha256.ComputeHash(data);
            return Convert.ToBase64String(hash);
        }

        public static string ComputeHexHash(byte[] data)
        {
            using var sha256 = SHA256.Create();
            byte[] hash = sha256.ComputeHash(data);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
        public class SecurityElement
        {
            public static string Escape(string str)
            {
                if (string.IsNullOrEmpty(str))
                    return str;
                
                return str
                    .Replace("&", "&amp;")
                    .Replace("<", "&lt;")
                    .Replace(">", "&gt;")
                    .Replace("\"", "&quot;")
                    .Replace("'", "&apos;");
            }
        }
              
        public static string FormatIssuerName(string fullIssuerName)
        {
            var elements = fullIssuerName.Split(',')
                .Select(x => x.Trim())
                .Where(x => x.StartsWith("CN=") || x.StartsWith("OU=") || x.StartsWith("O=") || x.StartsWith("C="))
                .OrderBy(x => {
                    // Urutan sesuai contoh: CN, OU, O, C
                    if (x.StartsWith("CN=")) return 1;
                    if (x.StartsWith("OU=")) return 2;
                    if (x.StartsWith("O=")) return 3;
                    if (x.StartsWith("C=")) return 4;
                    return 5;
                });
        
            return string.Join(", ", elements);
        }
    }
}