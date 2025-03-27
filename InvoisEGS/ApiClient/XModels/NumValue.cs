using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace InvoisEGS.ApiClient.XModels
{
    // Class untuk menyimpan nilai numerik
    public class NumValue
    {
        [JsonProperty("_", NullValueHandling = NullValueHandling.Ignore)]
        [XmlText]
        public decimal Value { get; set; }

        public NumValue() { }

        public NumValue(decimal numValue)
        {
            Value = numValue;
        }
    }
}
