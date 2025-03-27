using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace InvoisEGS.ApiClient.XModels
{
    // FreightAllowanceCharge
    public class FreightAllowanceCharge
    {
        [JsonProperty("ChargeIndicator", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "ChargeIndicator", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<BoolValue> ChargeIndicator { get; set; }

        [JsonProperty("AllowanceChargeReason", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "AllowanceChargeReason", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> AllowanceChargeReason { get; set; }

        [JsonProperty("Amount", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "Amount", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<Amount> Amount { get; set; }

        public FreightAllowanceCharge()
        {
            ChargeIndicator = new List<BoolValue>();
            AllowanceChargeReason = new List<TextValue>();
            Amount = new List<Amount>();
        }

        //public FreightAllowanceCharge(bool chargeIndicator, string reason, decimal amount, string currencyId) : this()
        //{
        //    SetChargeIndicator(chargeIndicator);
        //    SetAllowanceChargeReason(reason);
        //    SetAmount(amount, currencyId);
        //}

        //public void SetChargeIndicator(bool value) => 
        //    ChargeIndicator = new List<BoolValue> { new BoolValue(value) };

        //public bool? GetChargeIndicator() => 
        //    ChargeIndicator?.FirstOrDefault()?.Value;

        //public void SetAllowanceChargeReason(string reason) => 
        //    AllowanceChargeReason = new List<TextValue> { new TextValue(reason) };

        //public string GetAllowanceChargeReason() => 
        //    AllowanceChargeReason?.FirstOrDefault()?.Value;

        //public void SetAmount(decimal amount, string currencyId) => 
        //    Amount = new List<Amount> { new Amount(amount, currencyId) };

        //public Amount GetAmount() => 
        //    Amount?.FirstOrDefault();
    }
}
