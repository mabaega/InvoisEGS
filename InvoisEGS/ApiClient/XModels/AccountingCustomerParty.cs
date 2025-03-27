using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace InvoisEGS.ApiClient.XModels
{
    // AccountingCustomerParty
    public class AccountingCustomerParty
    {
        [JsonProperty("Party", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "Party", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<CustomerParty> Party { get; set; }

        public AccountingCustomerParty()
        {
            Party = new List<CustomerParty>();
        }

        //public void SetParty(CustomerParty party) => 
        //    Party = new List<CustomerParty> { party };

        //public CustomerParty GetParty() => 
        //    Party?.FirstOrDefault();

        //public void AddParty(CustomerParty party) => 
        //    Party.Add(party);

        //public void ClearParty() => 
        //    Party.Clear();
    }
}
