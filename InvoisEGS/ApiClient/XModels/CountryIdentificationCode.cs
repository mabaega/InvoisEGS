using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace InvoisEGS.ApiClient.XModels
{
    // CountryIdentificationCode
    public class CountryIdentificationCode
    {
        [JsonProperty("_", NullValueHandling = NullValueHandling.Ignore)]
        [XmlText]
        public string Value { get; set; } = "MYS";

        [JsonProperty("listID", NullValueHandling = NullValueHandling.Ignore)]
        [XmlAttribute("listID")]
        public string? ListID { get; set; } = "ISO3166-1";

        [JsonProperty("listAgencyID", NullValueHandling = NullValueHandling.Ignore)]
        [XmlAttribute("listAgencyID")]
        public string? ListAgencyID { get; set; } = "6";

        public CountryIdentificationCode(){}

        public CountryIdentificationCode(string value)
        {
            Value = value;
        }
    }
}
