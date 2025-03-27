using System.Xml.Serialization;

namespace InvoisEGS.ApiClient.Models.EInvoice;
[XmlRoot(ElementName = "FreightAllowanceCharge", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
public class FreightAllowanceCharge
{

    [XmlElement(ElementName = "ChargeIndicator", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
    public bool ChargeIndicator { get; set; }

    [XmlElement(ElementName = "AllowanceChargeReason", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
    public string AllowanceChargeReason { get; set; }

    [XmlElement(ElementName = "Amount", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
    public Amount Amount { get; set; }
}

