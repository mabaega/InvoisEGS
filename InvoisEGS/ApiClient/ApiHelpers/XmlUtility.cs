using InvoisEGS.ApiClient.XModels;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Security.Cryptography.Xml;

namespace InvoisEGS.ApiClient.ApiHelpers
{
    public static class XmlUtility
    {
        public static string SerializeToXml(MyInvoice invObject)
        {
            XmlSerializerNamespaces namespaces = new();
            namespaces.Add("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            namespaces.Add("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
            namespaces.Add("ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");

            XmlSerializer serializer = new(invObject.GetType());

            using MemoryStream memoryStream = new();
            XmlWriterSettings settings = new()
            {
                Indent = true,
                IndentChars = "  ",
                OmitXmlDeclaration = true,
                NamespaceHandling = NamespaceHandling.OmitDuplicates,
                Encoding = Encoding.UTF8,
            };

            using (XmlWriter xmlWriter = XmlWriter.Create(memoryStream, settings))
            {
                serializer.Serialize(xmlWriter, invObject, namespaces);
                xmlWriter.Flush();
            }

            memoryStream.Position = 0;
            string xmlString = new StreamReader(memoryStream).ReadToEnd().Trim();

            return xmlString;
        }
               
        public static string Canonicalize(string xmlContent)
        {
            string xmlResult = string.Empty;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlContent);

            //RemoveEmptyNodes(xmlDoc);

            XmlDsigC14NTransform transform = new XmlDsigC14NTransform();
            transform.LoadInput(xmlDoc);

            using (Stream outputStream = (Stream)transform.GetOutput(typeof(Stream)))
            using (StreamReader reader = new StreamReader(outputStream))
            {
                xmlResult = reader.ReadToEnd().Trim();
            }

            return xmlResult;
            //return Regex.Replace(xmlResult, @"\s+", " ").Trim();
        }
        private static void RemoveEmptyNodes(XmlNode node)
        {
            if (node == null) return;

            for (int i = node.ChildNodes.Count - 1; i >= 0; i--)
            {
                XmlNode childNode = node.ChildNodes[i];
                RemoveEmptyNodes(childNode);

                if (childNode.NodeType == XmlNodeType.Element &&
                    string.IsNullOrWhiteSpace(childNode.InnerText) &&
                    !childNode.HasChildNodes)
                {
                    node.RemoveChild(childNode);
                }
            }
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
            
            return xmlString;
        }

        public static string ReformatXml(XmlDocument xmlDoc)
        {
            using MemoryStream memoryStream = new();
            XmlWriterSettings settings = new()
            {
                Indent = true,
                IndentChars = "  ",
                OmitXmlDeclaration = true,
                Encoding = Encoding.UTF8,
            };

            using (XmlWriter xmlWriter = XmlWriter.Create(memoryStream, settings))
            {
                xmlDoc.Save(xmlWriter);
            }

            memoryStream.Position = 0;
            using StreamReader reader = new(memoryStream, Encoding.UTF8);
            return reader.ReadToEnd().Trim();
        }

        public static MyInvoice? LoadFromXmlFile(string filePath)
        {
            XmlSerializer serializer = new(typeof(MyInvoice));
            using FileStream fileStream = new(filePath, FileMode.Open, FileAccess.Read);
            using StreamReader streamReader = new(fileStream, Encoding.UTF8);
            using XmlReader xmlReader = XmlReader.Create(streamReader);
            return serializer.Deserialize(xmlReader) as MyInvoice;
        }

        public static void SaveToFile(MyInvoice invoice, string filePath)
        {
            string xmlContent = SerializeToXml(invoice);
            File.WriteAllText(filePath, xmlContent, new UTF8Encoding(false));
        }
    }

    public class StringWriterWithEncoding : StringWriter
    {
        private readonly Encoding encoding;

        public StringWriterWithEncoding(Encoding encoding) => this.encoding = encoding;

        public override Encoding Encoding => encoding;
    }
}
