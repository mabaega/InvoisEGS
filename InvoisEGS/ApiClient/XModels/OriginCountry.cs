using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace InvoisEGS.ApiClient.XModels
{
    public class OriginCountry
    {
        [JsonProperty("IdentificationCode", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "IdentificationCode", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<CountryIdentificationCode> ItemIdentificationCode { get; set; }

        public OriginCountry(){}

        public OriginCountry(string value)
        {
            ItemIdentificationCode = new List<CountryIdentificationCode> { new CountryIdentificationCode() { Value = value, ListAgencyID = null, ListID = null } };
        }
    }
}
