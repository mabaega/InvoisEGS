using System.Xml.Serialization;

namespace InvoisEGS.ApiClient.Models.EInvoice;
[XmlRoot(ElementName = "Country", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
public class Country
{

    [XmlElement(ElementName = "IdentificationCode", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
    public IdentificationCode IdentificationCode { get; set; }
}

