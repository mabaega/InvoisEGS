using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace InvoisEGS.ApiClient.XModels
{
    // Class untuk menyimpan nilai boolean
    public class BoolValue
    {
        [JsonProperty("_", NullValueHandling = NullValueHandling.Ignore)]
        [XmlText]
        public bool Value { get; set; }

        public BoolValue() { }

        public BoolValue(bool boolValue)
        {
            Value = boolValue;
        }
    }
}
