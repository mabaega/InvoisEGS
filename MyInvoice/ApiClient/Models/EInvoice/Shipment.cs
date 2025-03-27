using System.Xml.Serialization;

namespace MyInvois.ApiClient.Models.EInvoice;
[XmlRoot(ElementName = "Shipment", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
public class Shipment
{

    [XmlElement(ElementName = "ID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
    public ID ID { get; set; }

    [XmlElement(ElementName = "FreightAllowanceCharge", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
    public FreightAllowanceCharge FreightAllowanceCharge { get; set; }
}

