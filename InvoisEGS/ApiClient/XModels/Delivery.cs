using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace InvoisEGS.ApiClient.XModels
{
    // Delivery
    public class Delivery
    {
        [JsonProperty("DeliveryParty", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "DeliveryParty", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<DeliveryParty> DeliveryParty { get; set; }

        [JsonProperty("Shipment", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "Shipment", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<Shipment> Shipment { get; set; }

        public Delivery()
        {
            DeliveryParty = new List<DeliveryParty>();
            Shipment = new List<Shipment>();
        }

        //public void SetDeliveryParty(DeliveryParty party) => 
        //    DeliveryParty = new List<DeliveryParty> { party };

        //public DeliveryParty GetDeliveryParty() => 
        //    DeliveryParty?.FirstOrDefault();

        //public void SetShipment(Shipment shipment) => 
        //    Shipment = new List<Shipment> { shipment };

        //public Shipment GetShipment() => 
        //    Shipment?.FirstOrDefault();

        //public void AddShipment(Shipment shipment) => 
        //    Shipment.Add(shipment);
    }
}
