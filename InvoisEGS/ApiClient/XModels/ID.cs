using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace InvoisEGS.ApiClient.XModels
{
    public class ID
    {
        [XmlText]
        [JsonProperty("_", NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }

        [XmlAttribute("schemeID")]
        [JsonProperty("schemeID")]
        public string SchemeID { get; set; }

        [JsonProperty("schemeAgencyID", NullValueHandling = NullValueHandling.Ignore)]
        [XmlAttribute("schemeAgencyID")]
        public string? SchemeAgencyID { get; set; }

        public ID() { }

        public ID(string value)
        {
            Value = value;
        }

        public ID(string value, string schemeID)
        {
            Value = value;
            SchemeID = schemeID;
        }
        public ID(string value, string schemeID, string schemeAgencyID)
        {
            Value = value;
            SchemeID = schemeID;
            SchemeAgencyID = schemeAgencyID;
        }
    }
}
