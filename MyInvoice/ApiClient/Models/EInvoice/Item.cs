using System.Xml.Serialization;

namespace MyInvois.ApiClient.Models.EInvoice;
[XmlRoot(ElementName = "Item", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
public class Item
{

    [XmlElement(ElementName = "Description", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
    public string Description { get; set; }

    [XmlElement(ElementName = "OriginCountry", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
    public Country OriginCountry { get; set; }

    [XmlElement(ElementName = "CommodityClassification", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
    public CommodityClassification[] CommodityClassification { get; set; }
}

