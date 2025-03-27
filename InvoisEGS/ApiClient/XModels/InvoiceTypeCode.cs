using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace InvoisEGS.ApiClient.XModels
{
    // InvoiceTypeCode
    public class InvoiceTypeCode
    {
        [JsonProperty("_", NullValueHandling = NullValueHandling.Ignore)]
        [XmlText]
        public string Value { get; set; }

        [JsonProperty("listVersionID", NullValueHandling = NullValueHandling.Ignore)]
        [XmlAttribute("listVersionID")]
        public string ListVersionID { get; set; }

        public InvoiceTypeCode(){}

        public InvoiceTypeCode(string value, string listVersionID)
        {
            Value = value;
            ListVersionID = listVersionID;
        }
    }
}
