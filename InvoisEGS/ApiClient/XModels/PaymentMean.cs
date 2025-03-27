using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace InvoisEGS.ApiClient.XModels
{
    public class PaymentMean
    {
        [JsonProperty("PaymentMeansCode", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "PaymentMeansCode", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> PaymentMeansCode { get; set; }

        [JsonProperty("PayeeFinancialAccount", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "PayeeFinancialAccount", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<PayeeFinancialAccount> PayeeFinancialAccount { get; set; }

        public PaymentMean()
        {
            PaymentMeansCode = new List<TextValue>();
            PayeeFinancialAccount = new List<PayeeFinancialAccount>();
        }

        //public void SetPaymentMeansCode(string code) => 
        //    PaymentMeansCode = new List<TextValue> { new TextValue(code) };

        //public string GetPaymentMeansCode() => 
        //    PaymentMeansCode?.FirstOrDefault()?.Value;

        //public void SetPayeeFinancialAccount(PayeeFinancialAccount account) => 
        //    PayeeFinancialAccount = new List<PayeeFinancialAccount> { account };

        //public PayeeFinancialAccount GetPayeeFinancialAccount() => 
        //    PayeeFinancialAccount?.FirstOrDefault();
    }
}
