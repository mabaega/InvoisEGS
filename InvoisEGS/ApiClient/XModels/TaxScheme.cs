using System.Xml.Serialization;
using System.Xml;
using Newtonsoft.Json;

namespace InvoisEGS.ApiClient.XModels
{
    public class TaxScheme
    {
        [JsonProperty("ID", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement("ID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<ID> ID { get; set; }

        public TaxScheme() { }

        public TaxScheme(string value, string schemeID, string schemeAgencyID)
        {
            ID = new List<ID> { new ID(value, schemeID, schemeAgencyID) };
        }

    }
}
