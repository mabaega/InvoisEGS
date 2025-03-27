using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;
using System;

namespace InvoisEGS.ApiClient.XModels
{
    public class PartyIdentification
    {
        [JsonProperty("ID", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement("ID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<ID> ID { get; set; }

        public PartyIdentification() { }

        public PartyIdentification(string value, string schemeID)
        {
            ID = new List<ID> { new ID(value, schemeID, null) };
        }
    }

    //public class IdentificationID
    //{
    //    [XmlAttribute("schemeID")]
    //    [JsonProperty("schemeID")]
    //    public string SchemeID { get; set; }

    //    [XmlText]
    //    [JsonProperty("_")]
    //    public string Value { get; set; }

    //    public IdentificationID(string value, string schemeID)
    //    {
    //        Value = value;
    //        SchemeID = schemeID;
    //    }
    //}
}
