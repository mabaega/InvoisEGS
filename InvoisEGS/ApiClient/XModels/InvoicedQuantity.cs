using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace InvoisEGS.ApiClient.XModels
{
    // InvoicedQuantity
    public class InvoicedQuantity
    {
        [JsonProperty("_", NullValueHandling = NullValueHandling.Ignore)]
        [XmlText]
        public decimal Value { get; set; }

        [JsonProperty("unitCode", NullValueHandling = NullValueHandling.Ignore)]
        [XmlAttribute("unitCode")]
        public string UnitCode { get; set; }

        public InvoicedQuantity(){}

        public InvoicedQuantity(decimal value, string unitCode)
        {
            Value = value;
            UnitCode = unitCode;
        }
    }
}
