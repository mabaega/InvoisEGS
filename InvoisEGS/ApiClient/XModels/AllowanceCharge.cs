using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace InvoisEGS.ApiClient.XModels
{
    // AllowanceCharge
    public class AllowanceCharge
    {
        [JsonProperty("ChargeIndicator", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "ChargeIndicator", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<BoolValue> ChargeIndicator { get; set; }

        [JsonProperty("AllowanceChargeReason", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "AllowanceChargeReason", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> AllowanceChargeReason { get; set; }

        [JsonProperty("MultiplierFactorNumeric", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "MultiplierFactorNumeric", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<NumValue> MultiplierFactorNumeric { get; set; }

        [JsonProperty("Amount", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "Amount", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<Amount> Amount { get; set; }

        public AllowanceCharge()
        {
            ChargeIndicator = new List<BoolValue>();
            AllowanceChargeReason = new List<TextValue>();
            MultiplierFactorNumeric = new List<NumValue>();
            Amount = new List<Amount>();
        }

        public AllowanceCharge(bool chargeIndicator, string allowanceChargeReason, decimal multiplierFactorNumeric, decimal amount, string currencyID) : this()
        {
            SetChargeIndicator(chargeIndicator);
            SetAllowanceChargeReason(allowanceChargeReason);
            SetMultiplierFactorNumeric(multiplierFactorNumeric);
            SetAmount(amount, currencyID);
        }

        public void SetChargeIndicator(bool value) => 
            ChargeIndicator = new List<BoolValue> { new BoolValue(value) };

        //public bool? GetChargeIndicator() => 
        //    ChargeIndicator?.FirstOrDefault()?.Value;

        public void SetAllowanceChargeReason(string reason) => 
            AllowanceChargeReason = new List<TextValue> { new TextValue(reason) };

        //public string GetAllowanceChargeReason() => 
        //    AllowanceChargeReason?.FirstOrDefault()?.Value;

        public void SetMultiplierFactorNumeric(decimal factor) => 
            MultiplierFactorNumeric = new List<NumValue> { new NumValue(factor) };

        //public decimal? GetMultiplierFactorNumeric() => 
        //    MultiplierFactorNumeric?.FirstOrDefault()?.Value;

        public void SetAmount(decimal amount, string currencyId) => 
            Amount = new List<Amount> { new Amount(amount, currencyId) };

        //public Amount GetAmount() => 
        //    Amount?.FirstOrDefault();
    }
}
