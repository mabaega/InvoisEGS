using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace InvoisEGS.ApiClient.XModels
{
    public class TaxTotal
    {
        [JsonProperty("TaxAmount", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "TaxAmount", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<Amount> TaxAmount { get; set; }

        [JsonProperty("TaxSubtotal", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "TaxSubtotal", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<TaxSubtotal> TaxSubtotal { get; set; }

        public TaxTotal()
        {
            TaxAmount = new List<Amount>();
            TaxSubtotal = new List<TaxSubtotal>();
        }

        //public void SetTaxAmount(decimal amount, string currencyId) => 
        //    TaxAmount = new List<Amount> { new Amount(amount, currencyId) };

        //public Amount GetTaxAmount() => 
        //    TaxAmount?.FirstOrDefault();

        //public void SetTaxSubtotal(TaxSubtotal subtotal) => 
        //    TaxSubtotal = new List<TaxSubtotal> { subtotal };

        //public TaxSubtotal GetTaxSubtotal() => 
        //    TaxSubtotal?.FirstOrDefault();

        //public void AddTaxSubtotal(TaxSubtotal subtotal) => 
        //    TaxSubtotal.Add(subtotal);
    }
}
