using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace InvoisEGS.ApiClient.XModels
{
    // AdditionalDocumentReference
    public class AdditionalDocumentReference
    {
        [JsonProperty("ID", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "ID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<ID> ID { get; set; }

        [JsonProperty("UUID", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "UUID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<ID> UUID { get; set; }

        [JsonProperty("DocumentType", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "DocumentType", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> DocumentType { get; set; }

        [JsonProperty("DocumentDescription", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "DocumentDescription", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> DocumentDescription { get; set; }

        public AdditionalDocumentReference()
        {
            ID = new List<ID>();
            DocumentType = new List<TextValue>();
            DocumentDescription = new List<TextValue>();
        }

        public AdditionalDocumentReference(string id, string documentType, string documentDescription) : this()
        {
            SetId(id);
            SetDocumentType(documentType);
            SetDocumentDescription(documentDescription);
        }

        public AdditionalDocumentReference(string id, string documentType) : this()
        {
            SetId(id);
            SetDocumentType(documentType);
        }

        public void SetId(string id) => 
            ID = new List<ID> { new ID(id) };

        //public string GetId() => 
        //    ID?.FirstOrDefault()?.Value;

        public void SetDocumentType(string type) => 
            DocumentType = new List<TextValue> { new TextValue(type) };

        //public string GetDocumentType() => 
        //    DocumentType?.FirstOrDefault()?.Value;

        public void SetDocumentDescription(string description) => 
            DocumentDescription = new List<TextValue> { new TextValue(description) };

        //public string GetDocumentDescription() => 
        //    DocumentDescription?.FirstOrDefault()?.Value;
    }
}
