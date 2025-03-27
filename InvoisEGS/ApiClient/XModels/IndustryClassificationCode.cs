using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace InvoisEGS.ApiClient.XModels
{
    // IndustryClassificationCode
    public class IndustryClassificationCode
    {
        [JsonProperty("_", NullValueHandling = NullValueHandling.Ignore)]
        [XmlText]
        public string Value { get; set; } = "00000";

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        public IndustryClassificationCode(){}

        public IndustryClassificationCode(string value, string name)
        {
            Value = value;
            Name = name;
        }
    }
}
