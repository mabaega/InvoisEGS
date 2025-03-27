using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace InvoisEGS.ApiClient.XModels
{
    public class PaymentTerm
    {
        [JsonProperty("Note", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "Note", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> Note { get; set; }

        public PaymentTerm()
        {
            Note = new List<TextValue>();
        }

        public PaymentTerm(string note) : this()
        {
            SetNote(note);
        }

        public void SetNote(string note) => 
            Note = new List<TextValue> { new TextValue(note) };

        //public string GetNote() => 
        //    Note?.FirstOrDefault()?.Value;
    }
}
