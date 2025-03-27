using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace InvoisEGS.ApiClient.XModels
{
    // LegalMonetaryTotal
    public class LegalMonetaryTotal
    {
        [JsonProperty("LineExtensionAmount", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "LineExtensionAmount", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<Amount> LineExtensionAmount { get; set; }

        [JsonProperty("TaxExclusiveAmount", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "TaxExclusiveAmount", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<Amount> TaxExclusiveAmount { get; set; }

        [JsonProperty("TaxInclusiveAmount", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "TaxInclusiveAmount", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<Amount> TaxInclusiveAmount { get; set; }

        [JsonProperty("AllowanceTotalAmount", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "AllowanceTotalAmount", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<Amount> AllowanceTotalAmount { get; set; }

        [JsonProperty("ChargeTotalAmount", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "ChargeTotalAmount", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<Amount> ChargeTotalAmount { get; set; }

        [JsonProperty("PayableRoundingAmount", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "PayableRoundingAmount", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<Amount> PayableRoundingAmount { get; set; }

        [JsonProperty("PayableAmount", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "PayableAmount", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<Amount> PayableAmount { get; set; }

        public LegalMonetaryTotal()
        {
            LineExtensionAmount = new List<Amount>();
            TaxExclusiveAmount = new List<Amount>();
            TaxInclusiveAmount = new List<Amount>();
            AllowanceTotalAmount = new List<Amount>();
            ChargeTotalAmount = new List<Amount>();
            PayableRoundingAmount = new List<Amount>();
            PayableAmount = new List<Amount>();
        }

        public void SetLineExtensionAmount(decimal amount, string currencyId) => 
            LineExtensionAmount = new List<Amount> { new Amount(amount, currencyId) };

        //public Amount GetLineExtensionAmount() => 
        //    LineExtensionAmount?.FirstOrDefault();

        public void SetTaxExclusiveAmount(decimal amount, string currencyId) => 
            TaxExclusiveAmount = new List<Amount> { new Amount(amount, currencyId) };

        //public Amount GetTaxExclusiveAmount() => 
        //    TaxExclusiveAmount?.FirstOrDefault();

        public void SetTaxInclusiveAmount(decimal amount, string currencyId) => 
            TaxInclusiveAmount = new List<Amount> { new Amount(amount, currencyId) };

        //public Amount GetTaxInclusiveAmount() => 
        //    TaxInclusiveAmount?.FirstOrDefault();

        public void SetAllowanceTotalAmount(decimal amount, string currencyId) => 
            AllowanceTotalAmount = new List<Amount> { new Amount(amount, currencyId) };

        //public Amount GetAllowanceTotalAmount() => 
        //    AllowanceTotalAmount?.FirstOrDefault();

        public void SetChargeTotalAmount(decimal amount, string currencyId) => 
            ChargeTotalAmount = new List<Amount> { new Amount(amount, currencyId) };

        //public Amount GetChargeTotalAmount() => 
        //    ChargeTotalAmount?.FirstOrDefault();

        public void SetPayableRoundingAmount(decimal amount, string currencyId) => 
            PayableRoundingAmount = new List<Amount> { new Amount(amount, currencyId) };

        //public Amount GetPayableRoundingAmount() => 
        //    PayableRoundingAmount?.FirstOrDefault();

        public void SetPayableAmount(decimal amount, string currencyId) => 
            PayableAmount = new List<Amount> { new Amount(amount, currencyId) };

        //public Amount GetPayableAmount() => 
        //    PayableAmount?.FirstOrDefault();
    }
}
