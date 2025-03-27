using System.Xml.Serialization;

namespace InvoisEGS.ApiClient.Models.EInvoice;
[XmlRoot(ElementName = "AdditionalAccountID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
public class AdditionalAccountID
{

    [XmlAttribute(AttributeName = "schemeAgencyName")]
    public string SchemeAgencyName { get; set; }

    [XmlText]
    public string Text { get; set; }
}

