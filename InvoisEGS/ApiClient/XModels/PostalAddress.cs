using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace InvoisEGS.ApiClient.XModels
{
    public class PostalAddress
    {
        [JsonProperty("CityName", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "CityName", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> CityName { get; set; } = new List<TextValue>();

        [JsonProperty("PostalZone", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "PostalZone", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> PostalZone { get; set; }

        [JsonProperty("CountrySubentityCode", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "CountrySubentityCode", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> CountrySubentityCode { get; set; }

        [JsonProperty("AddressLine", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "AddressLine", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<AddressLine> AddressLine { get; set; }

        [JsonProperty("Country", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "Country", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<Country> Country { get; set; }

        public PostalAddress(){}

        //// Helper methods
        //public void SetCityName(string cityName)
        //{
        //    CityName = new List<TextValue> { new TextValue(cityName) };
        //}

        //public string GetCityName()
        //{
        //    return CityName?.FirstOrDefault()?.Value ?? string.Empty;
        //}

        //public void SetPostalZone(string postalZone)
        //{
        //    PostalZone = new List<TextValue> { new TextValue(postalZone) };
        //}

        //public string GetPostalZone()
        //{
        //    return PostalZone?.FirstOrDefault()?.Value ?? string.Empty;
        //}
    }
}
