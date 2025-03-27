using System.Xml.Serialization;

namespace MyInvois.ApiClient.Models.EInvoice;
[XmlRoot(ElementName = "ID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
public class ID
{
    [XmlAttribute(AttributeName = "schemeID")]
    public string SchemeID { get; set; }

    [XmlAttribute(AttributeName = "schemeAgencyID")]
    public string SchemeAgencyID { get; set; }

    [XmlText]
    public string Text { get; set; }
}

