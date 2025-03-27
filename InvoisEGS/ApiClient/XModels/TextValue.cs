using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace InvoisEGS.ApiClient.XModels
{
    // Class untuk menyimpan nilai teks
    public class TextValue
    {
        [JsonProperty("_", NullValueHandling = NullValueHandling.Ignore)]
        [XmlText]
        public string Value { get; set; }

        public TextValue() { }

        public TextValue(string value)
        {
            Value = value;
        }
    }
}
