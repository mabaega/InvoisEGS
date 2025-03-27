using System.Xml.Serialization;

namespace InvoisEGS.ApiClient.Models.EInvoice
{
    public class TaxExchangeRate
    {
        [XmlElement(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string SourceCurrencyCode { get; set; }

        [XmlElement(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string TargetCurrencyCode { get; set; }

        [XmlElement(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public decimal CalculationRate { get; set; }
    }
}