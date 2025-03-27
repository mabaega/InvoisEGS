using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace InvoisEGS.ApiClient.XModels
{
    // CommodityClassification
    public class CommodityClassification
    {
        [JsonProperty("ItemClassificationCode", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "ItemClassificationCode", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<ItemClassificationCode> ItemClassificationCode { get; set; }

        public CommodityClassification(){}

        public CommodityClassification(string value, string listID)
        {
            ItemClassificationCode = new List<ItemClassificationCode> { new ItemClassificationCode(value, listID) };
        }
    }
}
