using Org.BouncyCastle.Crypto;
using System.Numerics;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;

namespace InvoisEGS.ApiClient.Helpers
{
    public static class Utilities
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
        public static byte[] HashSha256(string rawData)
        {
            using SHA256 sha256 = SHA256.Create();
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
        }
        public static string GetSerialNumberForCertificateObject(X509Certificate2 x509Certificate2)
        {
            sbyte[] numArray = (from x in x509Certificate2.GetSerialNumber() select (sbyte)x).ToArray();
            BigInteger integer = new((byte[])(Array)numArray);
            return integer.ToString();
        }
        public static string HashSha256AsString(string rawData)
        {
            StringBuilder builder = new();
            foreach (byte num2 in HashSha256(rawData))
            {
                builder.Append(num2.ToString("x2"));
            }
            return builder.ToString();
        }
        public static string GetBase64InvoiceHash(string eInvoiceXml)
        {
            using MemoryStream stream = new(Encoding.UTF8.GetBytes(eInvoiceXml));
            XmlDsigC14NTransform transform1 = new(false);
            transform1.LoadInput(stream);
            MemoryStream output = transform1.GetOutput() as MemoryStream;
            byte[] hashBytes = HashSha256(Encoding.UTF8.GetString(output.ToArray()));
            return Convert.ToBase64String(hashBytes);
        }
        public static string GetSignedPropertiesHash(string signingTime, string digestValue, string x509IssuerName, string x509SerialNumber)
        {
            string xmlString = $@"<xades:QualifyingProperties xmlns:xades='http://uri.etsi.org/01903/v1.3.2#' xmlns:ds='http://www.w3.org/2000/09/xmldsig#' Target='signature'>
            <xades:SignedProperties Id='id-xades-signed-props'>
              <xades:SignedSignatureProperties>
                <xades:SigningTime>{signingTime}</xades:SigningTime>
                <xades:SigningCertificate>
                  <xades:Cert>
                    <xades:CertDigest>
                      <ds:DigestMethod Algorithm='http://www.w3.org/2001/04/xmlenc#sha256' />
                      <ds:DigestValue>{digestValue}</ds:DigestValue>
                    </xades:CertDigest>
                    <xades:IssuerSerial>
                      <ds:X509IssuerName>{x509IssuerName}</ds:X509IssuerName>
                      <ds:X509SerialNumber>{x509SerialNumber}</ds:X509SerialNumber>
                    </xades:IssuerSerial>
                  </xades:Cert>
                </xades:SigningCertificate>
              </xades:SignedSignatureProperties>
            </xades:SignedProperties>
          </xades:QualifyingProperties>";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = false;
            xmlDoc.LoadXml(xmlString);
            xmlDoc.Normalize();

            XmlNamespaceManager nsManager = new XmlNamespaceManager(xmlDoc.NameTable);
            nsManager.AddNamespace("xades", "http://uri.etsi.org/01903/v1.3.2#");
            XmlNode signedPropsNode = xmlDoc.SelectSingleNode("//xades:SignedProperties", nsManager);

            string signedPropertiesXml = signedPropsNode.OuterXml;

            byte[] signedBytes = Encoding.UTF8.GetBytes(signedPropertiesXml);
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(signedBytes);
                string hashBase64 = Convert.ToBase64String(hashBytes);
                return hashBase64;
            }
        }

        public static string GetDigitalSignature(string xmlHashing, string privateKeyContent)
        {
            try
            {
                byte[] buffer;
                byte[] hashBytes = Convert.FromBase64String(xmlHashing);

                // Verify input
                if (string.IsNullOrEmpty(privateKeyContent))
                {
                    throw new ArgumentException("Private key content is empty");
                }

                // Keep original formatting of the private key
               if (!privateKeyContent.Contains("-----BEGIN"))
                {
                    privateKeyContent = "-----BEGIN RSA PRIVATE KEY-----\n" + 
                                    privateKeyContent + 
                                    "\n-----END RSA PRIVATE KEY-----";
                }

                using (StringReader reader = new(privateKeyContent))
                {
                    Org.BouncyCastle.OpenSsl.PemReader pemReader = new(reader);
                    object pemObject = pemReader.ReadObject();

                    if (pemObject == null)
                    {
                        throw new InvalidOperationException("Failed to read PEM object");
                    }

                    AsymmetricKeyParameter privateKey;
                    if (pemObject is AsymmetricCipherKeyPair keyPair)
                    {
                        privateKey = keyPair.Private;
                    }
                    else if (pemObject is AsymmetricKeyParameter key)
                    {
                        privateKey = key;
                    }
                    else
                    {
                        throw new InvalidOperationException($"Unexpected key type: {pemObject.GetType().Name}");
                    }

                    ISigner signer = Org.BouncyCastle.Security.SignerUtilities.GetSigner("SHA256withRSA");
                    signer.Init(true, privateKey);
                    signer.BlockUpdate(hashBytes, 0, hashBytes.Length);
                    buffer = signer.GenerateSignature();
                }
                return Convert.ToBase64String(buffer);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to generate digital signature: {ex.Message}", ex);
            }
        }
    }
}