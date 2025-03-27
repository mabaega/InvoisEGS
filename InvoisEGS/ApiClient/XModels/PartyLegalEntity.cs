using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace InvoisEGS.ApiClient.XModels
{
    public class PartyLegalEntity
    {
        [JsonProperty("RegistrationName", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "RegistrationName", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> RegistrationName { get; set; }

        public PartyLegalEntity(){}

        public PartyLegalEntity(string value)
        {
            RegistrationName = new List<TextValue> { new TextValue(value) };
        }
    }
}
