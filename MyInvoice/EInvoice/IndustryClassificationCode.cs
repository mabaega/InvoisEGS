using System.Xml.Serialization;

namespace InvoisEGS.ApiClient.Models.EInvoice;
[XmlRoot(ElementName = "IndustryClassificationCode", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
public class IndustryClassificationCode
{

    [XmlAttribute(AttributeName = "name", Namespace = "")]
    public string Name { get; set; }

    [XmlText]
    public string Text { get; set; }

}