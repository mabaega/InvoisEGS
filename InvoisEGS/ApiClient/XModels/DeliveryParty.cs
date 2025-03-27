using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace InvoisEGS.ApiClient.XModels
{
    // DeliveryParty
    public class DeliveryParty
    {
        [JsonProperty("PartyLegalEntity", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "PartyLegalEntity", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<PartyLegalEntity> PartyLegalEntity { get; set; }

        [JsonProperty("PostalAddress", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "PostalAddress", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<PostalAddress> PostalAddress { get; set; }

        [JsonProperty("PartyIdentification", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "PartyIdentification", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<PartyIdentification> PartyIdentification { get; set; }

        public DeliveryParty(){}
    }
}
