using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace InvoisEGS.ApiClient.XModels
{
    // ItemPriceExtension
    public class ItemPriceExtension
    {
        [JsonProperty("Amount", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "Amount", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<Amount> Amount { get; set; }

        public ItemPriceExtension(){}
        public ItemPriceExtension(decimal amount, string currencyID)
        {
            Amount = new List<Amount> { new Amount(amount, currencyID) };
        }
    }
}
