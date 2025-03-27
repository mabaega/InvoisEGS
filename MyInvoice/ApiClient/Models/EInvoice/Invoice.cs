using System.Globalization;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace MyInvois.ApiClient.Models.EInvoice;
[XmlRoot(ElementName = "Invoice", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2")]
public class Invoice
{

    [XmlElement(ElementName = "ID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
    public ID ID { get; set; }

    [XmlElement(ElementName = "IssueDate", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
    public string IssueDate { get; set; }

    [XmlIgnore]
    [JsonIgnore]
    public DateTime IssueDateAsDateTime
    {
        get => DateTime.TryParseExact(IssueDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result) ? result : DateTime.MinValue;
        set => IssueDate = value.ToString("yyyy-MM-dd");
    }

    [XmlElement(ElementName = "IssueTime", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
    public string IssueTime { get; set; }

    [XmlIgnore]
    [JsonIgnore]
    public DateTime IssueTimeAsDateTime
    {
        get => DateTime.TryParseExact(IssueTime, "HH:mm:ssZ", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result) ? result : DateTime.MinValue;
        set => IssueTime = value.ToString("HH:mm:ssZ");
    }

    [XmlElement(ElementName = "InvoiceTypeCode", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
    public InvoiceTypeCode InvoiceTypeCode { get; set; }
    [XmlElement(ElementName = "DocumentCurrencyCode", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
    public string DocumentCurrencyCode { get; set; }

    [XmlElement(ElementName = "TaxCurrencyCode", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
    public string TaxCurrencyCode { get; set; }

    [XmlElement(ElementName = "InvoicePeriod", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
    public InvoicePeriod InvoicePeriod { get; set; }

    [XmlElement(ElementName = "BillingReference", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
    public BillingReference BillingReference { get; set; }

    [XmlElement(ElementName = "AdditionalDocumentReference", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
    public AdditionalDocumentReference[] AdditionalDocumentReference { get; set; }

    [XmlElement(ElementName = "AccountingSupplierParty", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
    public AccountingSupplierParty AccountingSupplierParty { get; set; }

    [XmlElement(ElementName = "AccountingCustomerParty", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
    public AccountingCustomerParty AccountingCustomerParty { get; set; }

    [XmlElement(ElementName = "Delivery", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
    public Delivery Delivery { get; set; }

    [XmlElement(ElementName = "PaymentMeans", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
    public PaymentMeans PaymentMeans { get; set; }

    [XmlElement(ElementName = "PaymentTerms", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
    public PaymentTerms PaymentTerms { get; set; }

    [XmlElement(ElementName = "PrepaidPayment", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
    public PrepaidPayment PrepaidPayment { get; set; }

    [XmlElement(ElementName = "AllowanceCharge", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
    public AllowanceCharge[] AllowanceCharge { get; set; }

    [XmlElement(ElementName = "TaxTotal", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
    public TaxTotal TaxTotal { get; set; }

    [XmlElement(ElementName = "LegalMonetaryTotal", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
    public LegalMonetaryTotal LegalMonetaryTotal { get; set; }

    [XmlElement(ElementName = "InvoiceLine", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
    public InvoiceLine[] InvoiceLine { get; set; }

    //   [XmlAttribute(AttributeName="xmlns")] 
    //public string Xmlns { get; set; }

    //   [XmlAttribute(AttributeName="cac", Namespace="http://www.w3.org/2000/xmlns/")] 
    //public string Cac { get; set; }

    //   [XmlAttribute(AttributeName="cbc", Namespace="http://www.w3.org/2000/xmlns/")] 
    //public string Cbc { get; set; }

    [XmlText]
    public string Text { get; set; }
}

