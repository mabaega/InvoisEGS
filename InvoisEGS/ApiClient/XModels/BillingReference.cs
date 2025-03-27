using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace InvoisEGS.ApiClient.XModels
{
    // BillingReference
    public class BillingReference
    {
        [JsonProperty("AdditionalDocumentReference", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "AdditionalDocumentReference", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<AdditionalDocumentReference> AdditionalDocumentReference { get; set; }

        public BillingReference()
        {
            AdditionalDocumentReference = new List<AdditionalDocumentReference>();
        }

        //public BillingReference(string id, string documentType, string documentDescription) : this()
        //{
        //    SetAdditionalDocumentReference(id, documentType, documentDescription);
        //}

        //public BillingReference(string id, string documentType) : this()
        //{
        //    SetAdditionalDocumentReference(id, documentType);
        //}

        //public void SetAdditionalDocumentReference(string id, string documentType, string documentDescription) => 
        //    AdditionalDocumentReference = new List<AdditionalDocumentReference> 
        //    { 
        //        new AdditionalDocumentReference(id, documentType, documentDescription) 
        //    };

        //public void SetAdditionalDocumentReference(string id, string documentType) => 
        //    AdditionalDocumentReference = new List<AdditionalDocumentReference> 
        //    { 
        //        new AdditionalDocumentReference(id, documentType) 
        //    };

        //public AdditionalDocumentReference GetAdditionalDocumentReference() => 
        //    AdditionalDocumentReference?.FirstOrDefault();

        //public void AddAdditionalDocumentReference(AdditionalDocumentReference reference) => 
        //    AdditionalDocumentReference.Add(reference);
    }
}
