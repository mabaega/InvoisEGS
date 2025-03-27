using InvoisEGS.ApiClient.Models.EInvoice;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;

namespace InvoisEGS.ApiClient.Helpers
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
        public static Invoice? DeserializeInvoice(string xmlContent)
        {
            XmlSerializer serializer = new(typeof(Invoice));
            using StringReader stringReader = new(xmlContent);
            using XmlReader xmlReader = XmlReader.Create(stringReader);
            return serializer.Deserialize(xmlReader) as Invoice;
        }

        public static string ObjectToXml(this object obj, XmlSerializerNamespaces namespaces)
        {
            XmlSerializer serializer = new(obj.GetType());

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
                serializer.Serialize(xmlWriter, obj, namespaces);
                xmlWriter.Flush();
            }

            memoryStream.Position = 0;
            string xmlString = new StreamReader(memoryStream).ReadToEnd().Trim();

            return xmlString;
        }

        public static string? GetCleanInvoiceXML(Invoice invObject)
        {
            try
            {
                XmlSerializerNamespaces namespaces = new();
                namespaces.Add("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
                namespaces.Add("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
                namespaces.Add("ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");

                string invoiceData = invObject.ObjectToXml(namespaces);
                //invoiceData = ApplyXSLT(invoiceData);

                return invoiceData;

            }
            catch (Exception)
            {
                //Console.WriteLine($"Error Get CleanInvoice XML: {ex.Message}");
                return null;
            }
        }
        public static Invoice? LoadFromXmlFile(string filePath)
        {
            //string xmlPath = @"C:\Tmp\one-doc.xml"; // Baca XML dari file
            //XmlDocument xmlDoc = new XmlDocument();
            //xmlDoc.PreserveWhitespace = true;
            //xmlDoc.Load(xmlPath);

            ///// Remove unnecessary elements
            //XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
            //RemoveElement(xmlDoc, "//Invoice//UBLExtensions", nsmgr);
            //RemoveElement(xmlDoc, "//Invoice//Signature", nsmgr);


            XmlSerializer serializer = new(typeof(Invoice));
            using FileStream fileStream = new(filePath, FileMode.Open, FileAccess.Read);
            using StreamReader streamReader = new(fileStream, Encoding.UTF8);
            using XmlReader xmlReader = XmlReader.Create(streamReader);
            return serializer.Deserialize(xmlReader) as Invoice;
        }

        //private static void RemoveElement(XmlDocument xmlDoc, string xpath, XmlNamespaceManager nsmgr)
        //{
        //    XmlNode node = xmlDoc.SelectSingleNode(xpath, nsmgr);
        //    if (node != null)
        //    {
        //        node.ParentNode.RemoveChild(node);
        //    }
        //}

        public static void SaveToFile(Invoice invoice, string filePath)
        {
            string xmlContent = SerializeInvoice(invoice);
            File.WriteAllText(filePath, xmlContent, new UTF8Encoding(false));
        }
    }
}