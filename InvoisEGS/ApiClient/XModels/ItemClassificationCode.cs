using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace InvoisEGS.ApiClient.XModels
{
    // ItemClassificationCode
    public class ItemClassificationCode
    {
        [JsonProperty("_", NullValueHandling = NullValueHandling.Ignore)]
        [XmlText]
        public string Value { get; set; }

        [JsonProperty("listID", NullValueHandling = NullValueHandling.Ignore)]
        [XmlAttribute("listID")]
        public string ListID { get; set; }

        public ItemClassificationCode(){}

        public ItemClassificationCode(string value, string listID)
        {
            Value = value;
            ListID = listID;
        }
    }
}
