using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace InvoisEGS.ApiClient.XModels
{
    public class TaxCategory
    {
        [JsonProperty("ID", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "ID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<ID> ID { get; set; }

        [JsonProperty("TaxExemptionReason", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "TaxExemptionReason", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> TaxExemptionReason { get; set; }

        [JsonProperty("TaxScheme", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "TaxScheme", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<TaxScheme> TaxScheme { get; set; }

        public TaxCategory()
        {
            ID = new List<ID>();
            TaxExemptionReason = new List<TextValue>();
            TaxScheme = new List<TaxScheme>();
        }

        public void SetId(string id) => 
            ID = new List<ID> { new ID(id) };

        //public string GetId() => 
        //    ID?.FirstOrDefault()?.Value;

        public void SetTaxExemptionReason(string reason) => 
            TaxExemptionReason = new List<TextValue> { new TextValue(reason) };

        //public string GetTaxExemptionReason() => 
        //    TaxExemptionReason?.FirstOrDefault()?.Value;

        public void SetTaxScheme(TaxScheme scheme) => 
            TaxScheme = new List<TaxScheme> { scheme };

        //public TaxScheme GetTaxScheme() => 
        //    TaxScheme?.FirstOrDefault();
    }
}
