using System.Xml.Serialization;

namespace MyInvois.ApiClient.Models.EInvoice;
// XmlSerializer serializer = new XmlSerializer(typeof(Invoice));
// using (StringReader reader = new StringReader(xml))
// {
//    var test = (Invoice)serializer.Deserialize(reader);
// }

[XmlRoot(ElementName = "InvoiceTypeCode", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
public class InvoiceTypeCode
{

    [XmlAttribute(AttributeName = "listVersionID")]
    public string ListVersionID { get; set; }

    [XmlText]
    public string Text { get; set; }
}

