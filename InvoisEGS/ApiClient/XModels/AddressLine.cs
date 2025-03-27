using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace InvoisEGS.ApiClient.XModels
{
    // AddressLine
    public class AddressLine
    {
        [JsonProperty("Line", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "Line", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> Line { get; set; }

        public AddressLine() { }

        public AddressLine(string line)
        {
            Line = new List<TextValue> { new TextValue { Value = line } };
        }
    }
}
