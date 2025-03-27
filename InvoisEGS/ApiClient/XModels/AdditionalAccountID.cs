using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace InvoisEGS.ApiClient.XModels
{
    // AdditionalAccountID
    public class AdditionalAccountID
    {
        [JsonProperty("_", NullValueHandling = NullValueHandling.Ignore)]
        [XmlText]
        public string Value { get; set; }

        [JsonProperty("schemeAgencyName", NullValueHandling = NullValueHandling.Ignore)]
        [XmlAttribute("schemeAgencyName")]
        public string schemeAgencyName { get; set; }

        public AdditionalAccountID(){}

        public AdditionalAccountID(string value, string schemeAgencyName)
        {
            Value = value;
            this.schemeAgencyName = schemeAgencyName;
        }
    }
}
