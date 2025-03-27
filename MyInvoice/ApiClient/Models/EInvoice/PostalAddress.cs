using System.Xml.Serialization;

namespace MyInvois.ApiClient.Models.EInvoice;
[XmlRoot(ElementName = "PostalAddress", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
public class PostalAddress
{

    [XmlElement(ElementName = "CityName", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
    public string CityName { get; set; }

    [XmlElement(ElementName = "PostalZone", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
    public int PostalZone { get; set; }

    [XmlElement(ElementName = "CountrySubentityCode", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
    public int CountrySubentityCode { get; set; }

    [XmlElement(ElementName = "AddressLine", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
    public AddressLine[] AddressLine { get; set; }

    [XmlElement(ElementName = "Country", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
    public Country Country { get; set; }
}

