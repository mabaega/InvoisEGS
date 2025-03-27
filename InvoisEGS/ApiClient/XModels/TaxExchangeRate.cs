using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace InvoisEGS.ApiClient.XModels
{
    public class TaxExchangeRate
    {
        [JsonProperty("SourceCurrencyCode", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string SourceCurrencyCode { get; set; }

        [JsonProperty("TargetCurrencyCode", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string TargetCurrencyCode { get; set; }

        [JsonProperty("CalculationRate", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public decimal CalculationRate { get; set; }

        public TaxExchangeRate(){}

        public TaxExchangeRate(string sourceCurrencyCode, string targetCurrencyCode, decimal calculationRate)
        {
            SourceCurrencyCode = sourceCurrencyCode;
            TargetCurrencyCode = targetCurrencyCode;
            CalculationRate = calculationRate;
        }
    }
}
