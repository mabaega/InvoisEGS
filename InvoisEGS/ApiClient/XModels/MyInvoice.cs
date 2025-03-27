using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace InvoisEGS.ApiClient.XModels
{

        public class NamespaceRoot
    {
        public NamespaceRoot()
        {
            _D = "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2";
            _A = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2";
            _B = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2";
            MyInvoice = new List<MyInvoice>();
        }

        [JsonProperty("_D", NullValueHandling = NullValueHandling.Ignore)]
        public string _D { get; set; }

        [JsonProperty("_A", NullValueHandling = NullValueHandling.Ignore)]
        public string _A { get; set; }

        [JsonProperty("_B", NullValueHandling = NullValueHandling.Ignore)]
        public string _B { get; set; }

        [JsonProperty("Invoice", NullValueHandling = NullValueHandling.Ignore)]
        public List<MyInvoice> MyInvoice { get; set; }
    }

    [XmlRoot(ElementName = "Invoice", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2")]
    public class MyInvoice
    {
        [JsonProperty("ID", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "ID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<ID> ID { get; set; }

        [JsonProperty("IssueDate", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "IssueDate", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> IssueDate { get; set; }

        [JsonProperty("IssueTime", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "IssueTime", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> IssueTime { get; set; }

        [JsonProperty("InvoiceTypeCode", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "InvoiceTypeCode", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<InvoiceTypeCode> InvoiceTypeCode { get; set; }

        [JsonProperty("DocumentCurrencyCode", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "DocumentCurrencyCode", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> DocumentCurrencyCode { get; set; }

        [JsonProperty("TaxCurrencyCode", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "TaxCurrencyCode", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> TaxCurrencyCode { get; set; }

        [JsonProperty("TaxExchangeRate", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "TaxExchangeRate", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public TaxExchangeRate TaxExchangeRate { get; set; }

        [JsonProperty("InvoicePeriod", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "InvoicePeriod", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<InvoicePeriod> InvoicePeriod { get; set; }

        [JsonProperty("BillingReference", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "BillingReference", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<BillingReference> BillingReference { get; set; }

        [JsonProperty("AdditionalDocumentReference", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "AdditionalDocumentReference", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<AdditionalDocumentReference> AdditionalDocumentReference { get; set; }

        [JsonProperty("AccountingSupplierParty", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "AccountingSupplierParty", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<AccountingSupplierParty> AccountingSupplierParty { get; set; }

        [JsonProperty("AccountingCustomerParty", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "AccountingCustomerParty", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<AccountingCustomerParty> AccountingCustomerParty { get; set; }

        [JsonProperty("Delivery", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "Delivery", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<Delivery> Delivery { get; set; }

        [JsonProperty("PaymentMeans", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "PaymentMeans", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<PaymentMean> PaymentMeans { get; set; }

        [JsonProperty("PaymentTerms", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "PaymentTerms", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<PaymentTerm> PaymentTerms { get; set; }

        [JsonProperty("PrepaidPayment", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "PrepaidPayment", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<PrepaidPayment> PrepaidPayment { get; set; }

        [JsonProperty("AllowanceCharge", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "AllowanceCharge", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<AllowanceCharge> AllowanceCharge { get; set; }

        [JsonProperty("TaxTotal", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "TaxTotal", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<TaxTotal> TaxTotal { get; set; }

        [JsonProperty("LegalMonetaryTotal", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "LegalMonetaryTotal", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<LegalMonetaryTotal> LegalMonetaryTotal { get; set; }

        [JsonProperty("InvoiceLine", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "InvoiceLine", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<InvoiceLine> InvoiceLine { get; set; }

        //[XmlIgnore]
        //public XmlSerializerNamespaces Xmlns { get; set; }

        public MyInvoice()
        {
            //Xmlns = new XmlSerializerNamespaces(new[]
            //{
            //    new XmlQualifiedName("", "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2"),
            //    new XmlQualifiedName("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2"),
            //    new XmlQualifiedName("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")
            //});
            ID = new List<ID>();
            IssueDate = new List<TextValue>();
            IssueTime = new List<TextValue>();
            InvoiceTypeCode = new List<InvoiceTypeCode>();
            DocumentCurrencyCode = new List<TextValue>();
            TaxCurrencyCode = new List<TextValue>();
            //TaxExchangeRate = new TaxExchangeRate();
            //InvoicePeriod = new List<InvoicePeriod>();
            //BillingReference = new List<BillingReference>();
            //AdditionalDocumentReference = new List<AdditionalDocumentReference>();
            AccountingSupplierParty = new List<AccountingSupplierParty>();
            AccountingCustomerParty = new List<AccountingCustomerParty>();
            //Delivery = new List<Delivery>();
            //PaymentMeans = new List<PaymentMean>();
            //PaymentTerms = new List<PaymentTerm>();
            //PrepaidPayment = new List<PrepaidPayment>();
            AllowanceCharge = new List<AllowanceCharge>();
            TaxTotal = new List<TaxTotal>();
            LegalMonetaryTotal = new List<LegalMonetaryTotal>();
            InvoiceLine = new List<InvoiceLine>();
        }

        // Basic Info
        //public void SetId(string id) =>
        //    ID = new List<TextValue> { new TextValue(id) };

        //public void SetIssueDate(string date) =>
        //    IssueDate = new List<TextValue> { new TextValue(date) };

        //public void SetIssueTime(string time) =>
        //    IssueTime = new List<TextValue> { new TextValue(time) };

        //public void SetInvoiceTypeCode(string code, string name = null) =>
        //    InvoiceTypeCode = new List<InvoiceTypeCode> { new InvoiceTypeCode(code, name) };

        //public void SetDocumentCurrencyCode(string code) =>
        //    DocumentCurrencyCode = new List<TextValue> { new TextValue(code) };

        //public void SetTaxCurrencyCode(string code) =>
        //    TaxCurrencyCode = new List<TextValue> { new TextValue(code) };

        //// Parties
        //public void SetSupplierParty(AccountingSupplierParty party) =>
        //    AccountingSupplierParty = new List<AccountingSupplierParty> { party };

        //public void SetCustomerParty(AccountingCustomerParty party) =>
        //    AccountingCustomerParty = new List<AccountingCustomerParty> { party };

        //// Payment Info
        //public void SetPaymentMeans(PaymentMean paymentMean) =>
        //    PaymentMeans = new List<PaymentMean> { paymentMean };

        //public void SetPaymentTerms(PaymentTerm terms) =>
        //    PaymentTerms = new List<PaymentTerm> { terms };

        //public void SetPrepaidPayment(PrepaidPayment payment) =>
        //    PrepaidPayment = new List<PrepaidPayment> { payment };

        //// Tax and Monetary
        //public void SetTaxTotal(TaxTotal tax) =>
        //    TaxTotal = new List<TaxTotal> { tax };

        //public void SetLegalMonetaryTotal(LegalMonetaryTotal total) =>
        //    LegalMonetaryTotal = new List<LegalMonetaryTotal> { total };

        //// Invoice Lines
        //public void AddInvoiceLine(InvoiceLine line) =>
        //    InvoiceLine.Add(line);

        //// Getters
        //public string GetId() =>
        //    ID?.FirstOrDefault()?.Value;

        //public string GetIssueDate() =>
        //    IssueDate?.FirstOrDefault()?.Value;

        //public string GetIssueTime() =>
        //    IssueTime?.FirstOrDefault()?.Value;

        //public string GetDocumentCurrencyCode() =>
        //    DocumentCurrencyCode?.FirstOrDefault()?.Value;

        //public AccountingSupplierParty GetSupplierParty() =>
        //    AccountingSupplierParty?.FirstOrDefault();

        //public AccountingCustomerParty GetCustomerParty() =>
        //    AccountingCustomerParty?.FirstOrDefault();

        //public PaymentMean GetPaymentMeans() =>
        //    PaymentMeans?.FirstOrDefault();

        //public TaxTotal GetTaxTotal() =>
        //    TaxTotal?.FirstOrDefault();

        //public LegalMonetaryTotal GetLegalMonetaryTotal() =>
        //    LegalMonetaryTotal?.FirstOrDefault();
    }
}
