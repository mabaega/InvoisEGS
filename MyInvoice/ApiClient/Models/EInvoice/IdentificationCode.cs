using System.Xml.Serialization;

namespace MyInvois.ApiClient.Models.EInvoice;
[XmlRoot(ElementName = "IdentificationCode", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
public class IdentificationCode
{

    [XmlAttribute(AttributeName = "listID")]
    public string ListID { get; set; }

    [XmlAttribute(AttributeName = "listAgencyID")]
    public int ListAgencyID { get; set; }

    [XmlText]
    public string Text { get; set; }
}

