using System.Xml.Serialization;

namespace MyInvois.ApiClient.Models.EInvoice;
[XmlRoot(ElementName = "TaxSubtotal", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
public class TaxSubtotal
{

    [XmlElement(ElementName = "TaxableAmount", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
    public Amount TaxableAmount { get; set; }

    [XmlElement(ElementName = "TaxAmount", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
    public Amount TaxAmount { get; set; }

    [XmlElement(ElementName = "TaxCategory", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
    public TaxCategory TaxCategory { get; set; }

    [XmlElement(ElementName = "Percent", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
    public double Percent { get; set; }
}

