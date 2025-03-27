using System.Xml.Serialization;

namespace InvoisEGS.ApiClient.Models.EInvoice;
[XmlRoot(ElementName = "PaymentMeans", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
public class PaymentMeans
{

    [XmlElement(ElementName = "PaymentMeansCode", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
    public int PaymentMeansCode { get; set; }

    [XmlElement(ElementName = "PayeeFinancialAccount", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
    public PayeeFinancialAccount PayeeFinancialAccount { get; set; }
}

