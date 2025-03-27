using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace InvoisEGS.ApiClient.XModels
{
    public class PrepaidPayment
    {
        [JsonProperty("ID", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "ID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<ID> ID { get; set; }

        [JsonProperty("PaidAmount", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "PaidAmount", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<Amount> PaidAmount { get; set; }

        [JsonProperty("PaidDate", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "PaidDate", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> PaidDate { get; set; }

        [JsonProperty("PaidTime", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "PaidTime", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> PaidTime { get; set; }

        public PrepaidPayment()
        {
            ID = new List<ID>();
            PaidAmount = new List<Amount>();
            PaidDate = new List<TextValue>();
            PaidTime = new List<TextValue>();
        }

        //public void SetId(string id) => 
        //    ID = new List<TextValue> { new TextValue(id) };

        //public string GetId() => 
        //    ID?.FirstOrDefault()?.Value;

        //public void SetPaidAmount(decimal amount, string currencyId) => 
        //    PaidAmount = new List<Amount> { new Amount(amount, currencyId) };

        //public Amount GetPaidAmount() => 
        //    PaidAmount?.FirstOrDefault();

        //public void SetPaidDate(string date) => 
        //    PaidDate = new List<TextValue> { new TextValue(date) };

        //public string GetPaidDate() => 
        //    PaidDate?.FirstOrDefault()?.Value;

        //public void SetPaidTime(string time) => 
        //    PaidTime = new List<TextValue> { new TextValue(time) };

        //public string GetPaidTime() => 
        //    PaidTime?.FirstOrDefault()?.Value;
    }
}
