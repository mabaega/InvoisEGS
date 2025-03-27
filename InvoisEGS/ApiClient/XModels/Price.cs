using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace InvoisEGS.ApiClient.XModels
{
    public class Price
    {
        [JsonProperty("PriceAmount", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "PriceAmount", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<Amount> PriceAmount { get; set; }

        public Price(){}

        public Price(decimal priceAmount, string currencyID)
        {
            PriceAmount = new List<Amount> { new Amount(priceAmount, currencyID) };
        }
    }
}
