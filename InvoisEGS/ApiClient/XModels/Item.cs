using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace InvoisEGS.ApiClient.XModels
{
    // Item
    public class Item
    {
        [JsonProperty("Description", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "Description", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> Description { get; set; }

        [JsonProperty("CommodityClassification", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "CommodityClassification", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<CommodityClassification> CommodityClassification { get; set; }

        [JsonProperty("OriginCountry", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "OriginCountry", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<OriginCountry> OriginCountry { get; set; }

        public Item()
        {
            CommodityClassification = new List<CommodityClassification>();
            Description = new List<TextValue>();
            OriginCountry = new List<OriginCountry>();
        }

        public Item(string description) : this()
        {
            SetDescription(description);
        }

        public void SetCommodityClassification(CommodityClassification classification) => 
            CommodityClassification = new List<CommodityClassification> { classification };

        //public CommodityClassification GetCommodityClassification() => 
        //    CommodityClassification?.FirstOrDefault();

        public void SetDescription(string description) => 
            Description = new List<TextValue> { new TextValue(description) };

        //public string GetDescription() => 
        //    Description?.FirstOrDefault()?.Value;

        //public void SetOriginCountry(OriginCountry country) => 
        //    OriginCountry = new List<OriginCountry> { country };

        //public OriginCountry GetOriginCountry() => 
        //    OriginCountry?.FirstOrDefault();

        //public void AddCommodityClassification(CommodityClassification classification) => 
        //    CommodityClassification.Add(classification);
    }
}
