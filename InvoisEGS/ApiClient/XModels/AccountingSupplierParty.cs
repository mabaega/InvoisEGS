using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace InvoisEGS.ApiClient.XModels
{
    // [JsonProperty("UBLExtensions", NullValueHandling = NullValueHandling.Ignore)]
    // public List<JsonExtModels.UBLExtension> UBLExtensions { get; set; }

    // [JsonProperty("Signature", NullValueHandling = NullValueHandling.Ignore)]
    // public List<JsonExtModels.RootSignature> Signature { get; set; }



    // AccountingSupplierParty
    public class AccountingSupplierParty
    {
        [JsonProperty("AdditionalAccountID", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "AdditionalAccountID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<AdditionalAccountID> AdditionalAccountID { get; set; }

        [JsonProperty("Party", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "Party", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<SupplierParty> Party { get; set; }

        public AccountingSupplierParty()
        {
            AdditionalAccountID = new List<AdditionalAccountID>();
            Party = new List<SupplierParty>();
        }

        //public void SetParty(SupplierParty party) => 
        //    Party = new List<SupplierParty> { party };

        //public SupplierParty GetParty() => 
        //    Party?.FirstOrDefault();

        //public void SetAdditionalAccountID(AdditionalAccountID accountId) => 
        //    AdditionalAccountID = new List<AdditionalAccountID> { accountId };

        //public AdditionalAccountID GetAdditionalAccountID() => 
        //    AdditionalAccountID?.FirstOrDefault();
    }
}
