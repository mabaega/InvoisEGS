using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace InvoisEGS.ApiClient.XModels
{
    public class PayeeFinancialAccount
    {
        [JsonProperty("ID", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "ID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<ID> ID { get; set; }

        public PayeeFinancialAccount(){}

        public PayeeFinancialAccount(string value)
        {
            ID = new List<ID> { new ID(value) };
        }
    }
}
