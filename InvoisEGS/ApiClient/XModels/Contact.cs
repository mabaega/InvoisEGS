using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace InvoisEGS.ApiClient.XModels
{
    // Contact
    public class Contact
    {
        [JsonProperty("Telephone", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "Telephone", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> Telephone { get; set; }

        [JsonProperty("ElectronicMail", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "ElectronicMail", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> ElectronicMail { get; set; }

        public Contact(){}

        public Contact(string telephone, string email)
        {
            Telephone = new List<TextValue> { new TextValue(telephone) };
            ElectronicMail = new List<TextValue> { new TextValue(email) };
        }
    }
}
