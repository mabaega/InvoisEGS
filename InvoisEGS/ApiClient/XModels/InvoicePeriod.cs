using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace InvoisEGS.ApiClient.XModels
{
    // InvoicePeriod
    public class InvoicePeriod
    {
        [JsonProperty("StartDate", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "StartDate", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> StartDate { get; set; }

        [JsonProperty("EndDate", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "EndDate", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> EndDate { get; set; }

        [JsonProperty("Description", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "Description", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> Description { get; set; }

        public InvoicePeriod()
        {
            StartDate = new List<TextValue>();
            EndDate = new List<TextValue>();
            Description = new List<TextValue>();
        }

        //public InvoicePeriod(string startDate, string endDate, string description) : this()
        //{
        //    SetStartDate(startDate);
        //    SetEndDate(endDate);
        //    SetDescription(description);
        //}

        //public void SetStartDate(string date) => 
        //    StartDate = new List<TextValue> { new TextValue(date) };

        //public string GetStartDate() => 
        //    StartDate?.FirstOrDefault()?.Value;

        //public void SetEndDate(string date) => 
        //    EndDate = new List<TextValue> { new TextValue(date) };

        //public string GetEndDate() => 
        //    EndDate?.FirstOrDefault()?.Value;

        //public void SetDescription(string description) => 
        //    Description = new List<TextValue> { new TextValue(description) };

        //public string GetDescription() => 
        //    Description?.FirstOrDefault()?.Value;
    }
}
