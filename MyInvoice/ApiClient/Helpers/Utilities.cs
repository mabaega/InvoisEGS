using Org.BouncyCastle.Crypto;
using System.Numerics;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;

namespace MyInvois.ApiClient.Helpers
{
    public static class Utilities
    {
        public static string LoadEmbededResources(string resourceName)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            using Stream stream = assembly.GetManifestResourceStream(resourceName);
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
        public static string GetSignedPropertiesHash(string signingTime, string digestValue, string x509SupplierName, string x509SerialNumber)
        {
            string xmlString = $@"<xades:SignedProperties Id=""id-xades-signed-props"">
										<xades:SignedSignatureProperties>
											<xades:SigningTime>{signingTime}</xades:SigningTime>
											<xades:SigningCertificate>
												<xades:Cert>
													<xades:CertDigest>
														<ds:DigestMethod Algorithm=""http://www.w3.org/2001/04/xmlenc#sha256""/>
														<ds:DigestValue>{digestValue}</ds:DigestValue>
													</xades:CertDigest>
													<xades:SupplierSerial>
														<ds:X509SupplierName>{x509SupplierName}</ds:X509SupplierName>
														<ds:X509SerialNumber>{x509SerialNumber}</ds:X509SerialNumber>
													</xades:SupplierSerial>
												</xades:Cert>
											</xades:SigningCertificate>
										</xades:SignedSignatureProperties>
									</xades:SignedProperties>";

            //XDocument document = XDocument.Load(xmlString);
            //xmlString = document.ToString(SaveOptions.DisableFormatting);

            //byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(xmlString.Trim()));
            //string hashHex = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            //return Convert.ToBase64String(Encoding.UTF8.GetBytes(hashHex));

            return GetBase64InvoiceHash(xmlString);
        }

        internal static string GetDigitalSignature(string xmlHashing, string privateKeyContent)
        {
            byte[] buffer;
            sbyte[] numArray = (from x in Convert.FromBase64String(xmlHashing) select (sbyte)x).ToArray();
            privateKeyContent = privateKeyContent.Replace("\n", "").Replace("\t", "");
            if (!privateKeyContent.Contains("-----BEGIN EC PRIVATE KEY-----") && !privateKeyContent.Contains("-----END EC PRIVATE KEY-----"))
            {
                privateKeyContent = "-----BEGIN EC PRIVATE KEY-----\n" + privateKeyContent + "\n-----END EC PRIVATE KEY-----\n";
            }
            using (TextReader reader = new StringReader(privateKeyContent))
            {
                AsymmetricKeyParameter @private = ((AsymmetricCipherKeyPair)new Org.BouncyCastle.OpenSsl.PemReader(reader).ReadObject()).Private;
                ISigner signer = Org.BouncyCastle.Security.SignerUtilities.GetSigner("SHA-256withECDSA");
                signer.Init(true, @private);
                signer.BlockUpdate((byte[])(Array)numArray, 0, numArray.Length);
                buffer = signer.GenerateSignature();
            }
            return Convert.ToBase64String(buffer);
        }

    }
}