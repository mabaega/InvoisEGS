using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace InvoisEGS.ApiClient.XModels
{
    public class TaxSubtotal
    {
        [JsonProperty("TaxableAmount", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "TaxableAmount", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<Amount> TaxableAmount { get; set; }

        [JsonProperty("TaxAmount", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "TaxAmount", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<Amount> TaxAmount { get; set; }

        [JsonProperty("Percent", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "Percent", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<NumValue> Percent { get; set; }

        [JsonProperty("TaxCategory", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "TaxCategory", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<TaxCategory> TaxCategory { get; set; }

        public TaxSubtotal()
        {
            TaxableAmount = new List<Amount>();
            TaxAmount = new List<Amount>();
            Percent = new List<NumValue>();
            TaxCategory = new List<TaxCategory>();
        }

        public TaxSubtotal(decimal taxableAmount, decimal taxAmount, decimal percent, string currencyId) : this()
        {
            SetTaxableAmount(taxableAmount, currencyId);
            SetTaxAmount(taxAmount, currencyId);
            SetPercent(percent);
        }

        public void SetTaxableAmount(decimal amount, string currencyId) => 
            TaxableAmount = new List<Amount> { new Amount(amount, currencyId) };

        //public Amount GetTaxableAmount() => 
        //    TaxableAmount?.FirstOrDefault();

        public void SetTaxAmount(decimal amount, string currencyId) => 
            TaxAmount = new List<Amount> { new Amount(amount, currencyId) };

        //public Amount GetTaxAmount() => 
        //    TaxAmount?.FirstOrDefault();

        public void SetPercent(decimal percent) => 
            Percent = new List<NumValue> { new NumValue(percent) };

        //public decimal? GetPercent() => 
        //    Percent?.FirstOrDefault()?.Value;

        public void SetTaxCategory(TaxCategory category) => 
            TaxCategory = new List<TaxCategory> { category };

        //public TaxCategory GetTaxCategory() => 
        //    TaxCategory?.FirstOrDefault();
    }
}
