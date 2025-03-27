using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace InvoisEGS.ApiClient.XModels
{
    public class CustomerParty
    {
        [JsonProperty("PartyIdentification", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "PartyIdentification", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<PartyIdentification> PartyIdentification { get; set; }

        [JsonProperty("PostalAddress", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "PostalAddress", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<PostalAddress> PostalAddress { get; set; }

        [JsonProperty("PartyLegalEntity", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "PartyLegalEntity", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<PartyLegalEntity> PartyLegalEntity { get; set; }

        [JsonProperty("Contact", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "Contact", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<Contact> Contact { get; set; }

        public CustomerParty()
        {
            PostalAddress = new List<PostalAddress>();
            PartyLegalEntity = new List<PartyLegalEntity>();
            PartyIdentification = new List<PartyIdentification>();
            Contact = new List<Contact>();
        }

        //public void SetPostalAddress(PostalAddress address) => 
        //    PostalAddress = new List<PostalAddress> { address };

        //public PostalAddress GetPostalAddress() => 
        //    PostalAddress?.FirstOrDefault();

        //public void SetPartyLegalEntity(PartyLegalEntity entity) => 
        //    PartyLegalEntity = new List<PartyLegalEntity> { entity };

        //public PartyLegalEntity GetPartyLegalEntity() => 
        //    PartyLegalEntity?.FirstOrDefault();

        //public void SetPartyIdentification(PartyIdentification identification) => 
        //    PartyIdentification = new List<PartyIdentification> { identification };

        //public PartyIdentification GetPartyIdentification() => 
        //    PartyIdentification?.FirstOrDefault();

        //public void SetContact(Contact contact) => 
        //    Contact = new List<Contact> { contact };

        //public Contact GetContact() => 
        //    Contact?.FirstOrDefault();
    }
}
