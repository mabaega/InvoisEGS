using System.Xml.Serialization;

namespace MyInvois.ApiClient.Models.EInvoice;
[XmlRoot(ElementName = "ItemClassificationCode", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
public class ItemClassificationCode
{

    [XmlAttribute(AttributeName = "listID")]
    public string ListID { get; set; }

    [XmlText]
    public string Text { get; set; }
}

