using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace InvoisEGS.ApiClient.XModels
{
    // Country
    public class Country
    {
        [JsonProperty("IdentificationCode", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "IdentificationCode", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<CountryIdentificationCode> IdentificationCode { get; set; }

        public Country(){}

        public Country(string countryCode)
        {
            IdentificationCode = new List<CountryIdentificationCode> { new CountryIdentificationCode(countryCode) };
        }
    }
}
