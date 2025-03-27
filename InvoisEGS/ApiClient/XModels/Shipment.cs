using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace InvoisEGS.ApiClient.XModels
{
    public class Shipment
    {
        [JsonProperty("ID", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "ID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<ID> ID { get; set; }

        [JsonProperty("FreightAllowanceCharge", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "FreightAllowanceCharge", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<FreightAllowanceCharge> FreightAllowanceCharge { get; set; }

        public Shipment()
        {
            ID = new List<ID>();
            FreightAllowanceCharge = new List<FreightAllowanceCharge>();
        }

        //public void SetId(string id) => 
        //    ID = new List<TextValue> { new TextValue(id) };

        //public string GetId() => 
        //    ID?.FirstOrDefault()?.Value;

        //public void SetFreightAllowanceCharge(FreightAllowanceCharge charge) => 
        //    FreightAllowanceCharge = new List<FreightAllowanceCharge> { charge };

        //public FreightAllowanceCharge GetFreightAllowanceCharge() => 
        //    FreightAllowanceCharge?.FirstOrDefault();

        //public void AddFreightAllowanceCharge(FreightAllowanceCharge charge) => 
        //    FreightAllowanceCharge.Add(charge);
    }
}
