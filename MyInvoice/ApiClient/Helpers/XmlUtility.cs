using MyInvois.ApiClient.Models.EInvoice;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace MyInvois.ApiClient.Helpers
{
    public static class XmlUtility
    {
        public static string SerializeInvoice(Invoice invoice)
        {
            XmlSerializer serializer = new(invoice.GetType());

            XmlSerializerNamespaces namespaces = new();
            namespaces.Add("", "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2");
            namespaces.Add("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            namespaces.Add("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");

            using MemoryStream memoryStream = new();
            XmlWriterSettings settings = new()
            {
                Indent = true,
                IndentChars = "    ",
                OmitXmlDeclaration = false,
                Encoding = Encoding.UTF8,
            };

            using (XmlWriter xmlWriter = XmlWriter.Create(memoryStream, settings))
            {
                serializer.Serialize(xmlWriter, invoice, namespaces);
                xmlWriter.Flush();
            }

            memoryStream.Position = 0;
            string xmlString = new StreamReader(memoryStream).ReadToEnd().Trim();

            return xmlString;
        }
        public static Invoice DeserializeInvoice(string xmlContent)
        {
            XmlSerializer serializer = new(typeof(Invoice));
            using StringReader stringReader = new(xmlContent);
            using XmlReader xmlReader = XmlReader.Create(stringReader);
            return serializer.Deserialize(xmlReader) as Invoice;
        }

        public static string CleanUpInvoiceXML(Invoice InvObject)
        {
            try
            {
                string InvData = SerializeInvoice(InvObject);
                string xslFile = Utilities.LoadEmbededResources("MyInvoice.Resources.CleanUpXml.xsl");
                StringBuilder output = new();
                XmlWriterSettings settings = new()
                {
                    OmitXmlDeclaration = true,
                    Encoding = Encoding.UTF8,
                    ConformanceLevel = ConformanceLevel.Auto,
                };
                using (XmlWriter writer = XmlWriter.Create(output, settings))
                {
                    XmlReader stylesheet = XmlReader.Create(new StringReader(xslFile));
                    XmlReader input = XmlReader.Create(new StringReader(InvData));
                    input.Read();
                    System.Xml.Xsl.XslCompiledTransform transform1 = new();
                    transform1.Load(stylesheet);
                    transform1.Transform(input, writer);
                }
                return output.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static Invoice LoadFromFile(string filePath)
        {
            XmlSerializer serializer = new(typeof(Invoice));
            using FileStream fileStream = new(filePath, FileMode.Open, FileAccess.Read);
            using StreamReader streamReader = new(fileStream, Encoding.UTF8);
            using XmlReader xmlReader = XmlReader.Create(streamReader);
            return serializer.Deserialize(xmlReader) as Invoice;
        }

        public static void SaveToFile(Invoice invoice, string filePath)
        {
            string xmlContent = SerializeInvoice(invoice);
            File.WriteAllText(filePath, xmlContent, new UTF8Encoding(false));
        }
    }
}