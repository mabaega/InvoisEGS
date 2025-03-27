using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace InvoisEGS.ApiClient.XModels
{
    // Amount
    public class Amount
    {
        [JsonProperty("_", NullValueHandling = NullValueHandling.Ignore)]
        [XmlText]
        public decimal Value { get; set; } = 0;

        [JsonProperty("currencyID", NullValueHandling = NullValueHandling.Ignore)]
        [XmlAttribute("currencyID")]
        public string CurrencyID { get; set; } = "MYR";

        public Amount(){}
        public Amount(decimal value, string currencyID)
        {
            Value = value;
            CurrencyID = currencyID;
        }
    }
}
