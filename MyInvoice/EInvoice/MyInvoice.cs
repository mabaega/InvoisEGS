using Newtonsoft.Json;
using System.Globalization;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml;

namespace zzzInvoisEGS.ApiClient.XModels
{
    public class DecimalJsonConverter : JsonConverter<decimal>
    {
        public override decimal ReadJson(JsonReader reader, Type objectType, decimal existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.Value == null) return 0;
            return Convert.ToDecimal(reader.Value);
        }

        public override void WriteJson(JsonWriter writer, decimal value, JsonSerializer serializer)
        {
            string formatted = value.ToString("0.##########", CultureInfo.InvariantCulture);
            writer.WriteValue(formatted); // Gunakan WriteValue agar tetap valid JSON
                                          //writer.WriteRawValue(formatted);
        }
    }
    public class DecimalXmlConverter : IXmlSerializable
    {
        private decimal _value;

        public DecimalXmlConverter() { }

        public DecimalXmlConverter(decimal value)
        {
            _value = value;
        }

        public static implicit operator decimal(DecimalXmlConverter d) => d._value;
        public static implicit operator DecimalXmlConverter(decimal d) => new DecimalXmlConverter(d);

        public XmlSchema GetSchema() => null;

        public void ReadXml(XmlReader reader)
        {
            if (decimal.TryParse(reader.ReadElementContentAsString(), NumberStyles.Float, CultureInfo.InvariantCulture, out decimal result))
            {
                _value = result;
            }
            else
            {
                _value = 0; // Default jika parsing gagal
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteString(_value.ToString("0.##########", CultureInfo.InvariantCulture));
        }
    }

    public class Root
    {
        public Root()
        {
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
        public List<TextValue> ID { get; set; }

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

        public MyInvoice()
        {
            Xmlns = new XmlSerializerNamespaces(new[]
            {
                new XmlQualifiedName("", "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2"),
                new XmlQualifiedName("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2"),
                new XmlQualifiedName("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")
            });
            ID = new List<TextValue>();
            IssueDate = new List<TextValue>();
            IssueTime = new List<TextValue>();
            InvoiceTypeCode = new List<InvoiceTypeCode>();
            DocumentCurrencyCode = new List<TextValue>();
            TaxCurrencyCode = new List<TextValue>();
            TaxExchangeRate = new TaxExchangeRate();
            InvoicePeriod = new List<InvoicePeriod>();
            BillingReference = new List<BillingReference>();
            AdditionalDocumentReference = new List<AdditionalDocumentReference>();
            AccountingSupplierParty = new List<AccountingSupplierParty>();
            AccountingCustomerParty = new List<AccountingCustomerParty>();
            Delivery = new List<Delivery>();
            PaymentMeans = new List<PaymentMean>();
            PaymentTerms = new List<PaymentTerm>();
            PrepaidPayment = new List<PrepaidPayment>();
            AllowanceCharge = new List<AllowanceCharge>();
            TaxTotal = new List<TaxTotal>();
            LegalMonetaryTotal = new List<LegalMonetaryTotal>();
            InvoiceLine = new List<InvoiceLine>();
        }

        public XmlSerializerNamespaces Xmlns { get; set; }

    }

    public class TaxExchangeRate
    {
        [JsonProperty("SourceCurrencyCode", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string SourceCurrencyCode { get; set; }

        [JsonProperty("TargetCurrencyCode", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string TargetCurrencyCode { get; set; }

        [JsonProperty("CalculationRate", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public decimal CalculationRate { get; set; }

        public TaxExchangeRate(){}

        public TaxExchangeRate(string sourceCurrencyCode, string targetCurrencyCode, decimal calculationRate)
        {
            SourceCurrencyCode = sourceCurrencyCode;
            TargetCurrencyCode = targetCurrencyCode;
            CalculationRate = calculationRate;
        }
    }

    // [JsonProperty("UBLExtensions", NullValueHandling = NullValueHandling.Ignore)]
    // public List<JsonExtModels.UBLExtension> UBLExtensions { get; set; }

    // [JsonProperty("Signature", NullValueHandling = NullValueHandling.Ignore)]
    // public List<JsonExtModels.RootSignature> Signature { get; set; }



    // AccountingSupplierParty
    public class AccountingSupplierParty
    {
        [JsonProperty("AdditionalAccountID", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "AdditionalAccountID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<AdditionalAccountID> AdditionalAccountID { get; set; }

        [JsonProperty("Party", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "Party", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<SupplierParty> Party { get; set; }

        public AccountingSupplierParty(){}
    }

    // AccountingCustomerParty
    public class AccountingCustomerParty
    {
        [JsonProperty("Party", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "Party", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<CustomerParty> Party { get; set; }

        public AccountingCustomerParty(){}
    }

    // AdditionalAccountID
    public class AdditionalAccountID
    {
        [JsonProperty("_", NullValueHandling = NullValueHandling.Ignore)]
        [XmlText]
        public string Value { get; set; }

        [JsonProperty("schemeAgencyName", NullValueHandling = NullValueHandling.Ignore)]
        [XmlAttribute("schemeAgencyName")]
        public string schemeAgencyName { get; set; }

        public AdditionalAccountID(){}

        public AdditionalAccountID(string value, string schemeAgencyName)
        {
            Value = value;
            this.schemeAgencyName = schemeAgencyName;
        }
    }

    // AdditionalDocumentReference
    public class AdditionalDocumentReference
    {
        [JsonProperty("ID", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "ID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> ID { get; set; }

        [JsonProperty("DocumentType", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "DocumentType", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> DocumentType { get; set; }

        [JsonProperty("DocumentDescription", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "DocumentDescription", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> DocumentDescription { get; set; }

        public AdditionalDocumentReference(){}

        public AdditionalDocumentReference(string id, string documentType, string documentDescription)
        {
            ID = new List<TextValue> { new TextValue(id) };
            DocumentType = new List<TextValue> { new TextValue(documentType) };
            DocumentDescription = new List<TextValue> { new TextValue(documentDescription) };
        }

        public AdditionalDocumentReference(string id, string documentType)
        {
            ID = new List<TextValue> { new TextValue(id) };
            DocumentType = new List<TextValue> { new TextValue(documentType) };
        }
    }

    // AddressLine
    public class AddressLine
    {
        [JsonProperty("Line", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "Line", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> Line { get; set; }

        public AddressLine(){}

        public AddressLine(string line)
        {
            Line = new List<TextValue> { new TextValue(line) };
        }
    }

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

        public AllowanceCharge(){}

        public AllowanceCharge(bool chargeIndicator, string allowanceChargeReason, decimal multiplierFactorNumeric, decimal amount, string currencyID)
        {
            ChargeIndicator = new List<BoolValue> { new BoolValue(chargeIndicator) };
            AllowanceChargeReason = new List<TextValue> { new TextValue(allowanceChargeReason) };
            MultiplierFactorNumeric = new List<NumValue> { new NumValue(multiplierFactorNumeric) };
            Amount = new List<Amount> { new Amount(amount, currencyID) };
        }
    }

    // Amount
    public class Amount
    {
        [JsonProperty("_", NullValueHandling = NullValueHandling.Ignore)]
        [XmlText]
        public decimal Value { get; set; } = 0;

        [JsonProperty("currencyID", NullValueHandling = NullValueHandling.Ignore)]
        [XmlAttribute("currencyID")]
        public string CurrencyID { get; set; } = "MYR";

        public Amount(){}
        public Amount(decimal value, string currencyID)
        {
            Value = value;
            CurrencyID = currencyID;
        }
    }

    // BillingReference
    public class BillingReference
    {
        [JsonProperty("AdditionalDocumentReference", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "AdditionalDocumentReference", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<AdditionalDocumentReference> AdditionalDocumentReference { get; set; }

        public BillingReference(){}

        public BillingReference(string id, string documentType, string documentDescription)
        {
            AdditionalDocumentReference = new List<AdditionalDocumentReference> { new AdditionalDocumentReference(id, documentType, documentDescription) };
        }

        public BillingReference(string id, string documentType)
        {
            AdditionalDocumentReference = new List<AdditionalDocumentReference> { new AdditionalDocumentReference(id, documentType) };
        }
    }

    // CommodityClassification
    public class CommodityClassification
    {
        [JsonProperty("ItemClassificationCode", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "ItemClassificationCode", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<ItemClassificationCode> ItemClassificationCode { get; set; }

        public CommodityClassification(){}

        public CommodityClassification(string value, string listID)
        {
            ItemClassificationCode = new List<ItemClassificationCode> { new ItemClassificationCode(value, listID) };
        }
    }

    // Contact
    public class Contact
    {
        [JsonProperty("Telephone", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "Telephone", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> Telephone { get; set; }

        [JsonProperty("ElectronicMail", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "ElectronicMail", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> ElectronicMail { get; set; }

        public Contact(){}

        public Contact(string telephone, string email)
        {
            Telephone = new List<TextValue> { new TextValue(telephone) };
            ElectronicMail = new List<TextValue> { new TextValue(email) };
        }
    }

    // Country
    public class Country
    {
        [JsonProperty("IdentificationCode", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "IdentificationCode", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<CountryIdentificationCode> IdentificationCode { get; set; }

        public Country(){}

        public Country(string countryCode)
        {
            IdentificationCode = new List<CountryIdentificationCode> { new CountryIdentificationCode(countryCode) };
        }
    }

    // Delivery
    public class Delivery
    {
        [JsonProperty("DeliveryParty", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "DeliveryParty", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<DeliveryParty> DeliveryParty { get; set; }

        [JsonProperty("Shipment", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "Shipment", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<Shipment> Shipment { get; set; }

        public Delivery(){}
    }

    // DeliveryParty
    public class DeliveryParty
    {
        [JsonProperty("PartyLegalEntity", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "PartyLegalEntity", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<PartyLegalEntity> PartyLegalEntity { get; set; }

        [JsonProperty("PostalAddress", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "PostalAddress", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<PostalAddress> PostalAddress { get; set; }

        [JsonProperty("PartyIdentification", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "PartyIdentification", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<PartyIdentification> PartyIdentification { get; set; }

        public DeliveryParty(){}
    }

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

        public FreightAllowanceCharge(){}
    }

    // TaxSchemeId
    public class TaxSchemeId
    {
        [JsonProperty("_", NullValueHandling = NullValueHandling.Ignore)]
        [XmlText]
        public string Value { get; set; }

        [JsonProperty("schemeID", NullValueHandling = NullValueHandling.Ignore)]
        [XmlAttribute("schemeID")]
        public string? SchemeID { get; set; }

        [JsonProperty("schemeAgencyID", NullValueHandling = NullValueHandling.Ignore)]
        [XmlAttribute("schemeAgencyID")]
        public string? SchemeAgencyID { get; set; }

        public TaxSchemeId(){}

        public TaxSchemeId(string value, string schemeID, string schemeAgencyID)
        {
            Value = value;
            SchemeID = schemeID;
            SchemeAgencyID = schemeAgencyID;
        }
    }

    // PartyIdentificationID
    public class PartyIdentificationID
    {
        [JsonProperty("_", NullValueHandling = NullValueHandling.Ignore)]
        [XmlText]
        public string Value { get; set; }

        [JsonProperty("schemeID", NullValueHandling = NullValueHandling.Ignore)]
        [XmlAttribute("schemeID")]
        public string? SchemeID { get; set; }

        public PartyIdentificationID(){}

        public PartyIdentificationID(string value, string schemeID)
        {
            Value = value;
            SchemeID = schemeID;
        }
    }

    // CountryIdentificationCode
    public class CountryIdentificationCode
    {
        [JsonProperty("_", NullValueHandling = NullValueHandling.Ignore)]
        [XmlText]
        public string Value { get; set; } = "MYS";

        [JsonProperty("listID", NullValueHandling = NullValueHandling.Ignore)]
        [XmlAttribute("listID")]
        public string? ListID { get; set; } = "ISO3166-1";

        [JsonProperty("listAgencyID", NullValueHandling = NullValueHandling.Ignore)]
        [XmlAttribute("listAgencyID")]
        public string? ListAgencyID { get; set; } = "6";

        public CountryIdentificationCode(){}

        public CountryIdentificationCode(string value)
        {
            Value = value;
        }
    }

    // IndustryClassificationCode
    public class IndustryClassificationCode
    {
        [JsonProperty("_", NullValueHandling = NullValueHandling.Ignore)]
        [XmlText]
        public string Value { get; set; } = "00000";

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        public IndustryClassificationCode(){}

        public IndustryClassificationCode(string value, string name)
        {
            Value = value;
            Name = name;
        }
    }

    // InvoicedQuantity
    public class InvoicedQuantity
    {
        [JsonProperty("_", NullValueHandling = NullValueHandling.Ignore)]
        [XmlText]
        public Decimal Value { get; set; }

        [JsonProperty("unitCode", NullValueHandling = NullValueHandling.Ignore)]
        [XmlAttribute("unitCode")]
        public string UnitCode { get; set; }

        public InvoicedQuantity(){}

        public InvoicedQuantity(Decimal value, string unitCode)
        {
            Value = value;
            UnitCode = unitCode;
        }
    }

    // InvoiceLine
    public class InvoiceLine
    {
        [JsonProperty("ID", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "ID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> ID { get; set; }

        [JsonProperty("InvoicedQuantity", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "InvoicedQuantity", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<InvoicedQuantity> InvoicedQuantity { get; set; }

        [JsonProperty("LineExtensionAmount", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "LineExtensionAmount", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<Amount> LineExtensionAmount { get; set; }

        [JsonProperty("AllowanceCharge", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "AllowanceCharge", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<AllowanceCharge> AllowanceCharge { get; set; }

        [JsonProperty("TaxTotal", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "TaxTotal", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<TaxTotal> TaxTotal { get; set; }

        [JsonProperty("Item", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "Item", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<Item> Item { get; set; }

        [JsonProperty("Price", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "Price", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<Price> Price { get; set; }

        [JsonProperty("ItemPriceExtension", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "ItemPriceExtension", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<ItemPriceExtension> ItemPriceExtension { get; set; }

        public InvoiceLine(){}
    }

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

        public InvoicePeriod(){}

        public InvoicePeriod(string startDate, string endDate, string description)
        {
            StartDate = new List<TextValue> { new TextValue(startDate) };
            EndDate = new List<TextValue> { new TextValue(endDate) };
            Description = new List<TextValue> { new TextValue(description) };
        }
    }

    // InvoiceTypeCode
    public class InvoiceTypeCode
    {
        [JsonProperty("_", NullValueHandling = NullValueHandling.Ignore)]
        [XmlText]
        public string Value { get; set; }

        [JsonProperty("listVersionID", NullValueHandling = NullValueHandling.Ignore)]
        [XmlAttribute("listVersionID")]
        public string ListVersionID { get; set; }

        public InvoiceTypeCode(){}

        public InvoiceTypeCode(string value, string listVersionID)
        {
            Value = value;
            ListVersionID = listVersionID;
        }
    }

    // Item
    public class Item
    {
        [JsonProperty("CommodityClassification", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "CommodityClassification", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<CommodityClassification> CommodityClassification { get; set; }

        [JsonProperty("Description", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "Description", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> Description { get; set; }

        [JsonProperty("OriginCountry", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "OriginCountry", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<OriginCountry> OriginCountry { get; set; }

        public Item(){}
    }

    // ItemClassificationCode
    public class ItemClassificationCode
    {
        [JsonProperty("_", NullValueHandling = NullValueHandling.Ignore)]
        [XmlText]
        public string Value { get; set; }

        [JsonProperty("listID", NullValueHandling = NullValueHandling.Ignore)]
        [XmlAttribute("listID")]
        public string ListID { get; set; }

        public ItemClassificationCode(){}

        public ItemClassificationCode(string value, string listID)
        {
            Value = value;
            ListID = listID;
        }
    }

    // ItemPriceExtension
    public class ItemPriceExtension
    {
        [JsonProperty("Amount", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "Amount", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<Amount> Amount { get; set; }

        public ItemPriceExtension(){}
        public ItemPriceExtension(decimal amount, string currencyID)
        {
            Amount = new List<Amount> { new Amount(amount, currencyID) };
        }
    }

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

        public LegalMonetaryTotal(){}
    }

    public class OriginCountry
    {
        [JsonProperty("IdentificationCode", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "IdentificationCode", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<CountryIdentificationCode> IdentificationCode { get; set; }

        public OriginCountry(){}

        public OriginCountry(string value)
        {
            IdentificationCode = new List<CountryIdentificationCode> { new CountryIdentificationCode(value) };
        }
    }

    public class SupplierParty
    {
        [JsonProperty("IndustryClassificationCode", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "IndustryClassificationCode", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<IndustryClassificationCode>? IndustryClassificationCode { get; set; }

        [JsonProperty("PartyIdentification", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "PartyIdentification", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<PartyIdentification> PartyIdentification { get; set; }

        [JsonProperty("PostalAddress", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "PostalAddress", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<PostalAddress> PostalAddress { get; set; }

        [JsonProperty("PartyLegalEntity", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "PartyLegalEntity", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<PartyLegalEntity> PartyLegalEntity { get; set; }

        [JsonProperty("Contact", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "Contact", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<Contact> Contact { get; set; }

        public SupplierParty(){}
    }

    public class CustomerParty
    {
        [JsonProperty("PostalAddress", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "PostalAddress", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<PostalAddress> PostalAddress { get; set; }

        [JsonProperty("PartyLegalEntity", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "PartyLegalEntity", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<PartyLegalEntity> PartyLegalEntity { get; set; }

        [JsonProperty("PartyIdentification", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "PartyIdentification", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<PartyIdentification> PartyIdentification { get; set; }

        [JsonProperty("Contact", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "Contact", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<Contact> Contact { get; set; }

        public CustomerParty(){}
    }

    public class PartyIdentification
    {
        [JsonProperty("ID", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "ID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<PartyIdentificationID> ID { get; set; }

        public PartyIdentification(){}

        public PartyIdentification(string value, string schemeID)
        {
            ID = new List<PartyIdentificationID> { new PartyIdentificationID(value, schemeID) };
        }

    }

    public class PartyLegalEntity
    {
        [JsonProperty("RegistrationName", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "RegistrationName", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> RegistrationName { get; set; }

        public PartyLegalEntity(){}

        public PartyLegalEntity(string value)
        {
            RegistrationName = new List<TextValue> { new TextValue(value) };
        }
    }

    public class PayeeFinancialAccount
    {
        [JsonProperty("ID", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "ID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> ID { get; set; }

        public PayeeFinancialAccount(){}

        public PayeeFinancialAccount(string value)
        {
            ID = new List<TextValue> { new TextValue(value) };
        }
    }

    public class PaymentMean
    {
        [JsonProperty("PaymentMeansCode", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "PaymentMeansCode", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> PaymentMeansCode { get; set; }

        [JsonProperty("PayeeFinancialAccount", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "PayeeFinancialAccount", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<PayeeFinancialAccount> PayeeFinancialAccount { get; set; }

        public PaymentMean(){}

        public PaymentMean(string paymentMeansCode, string payeeFinancialAccount)
        {
            PaymentMeansCode = new List<TextValue> { new TextValue(paymentMeansCode) };
            PayeeFinancialAccount = new List<PayeeFinancialAccount> { new PayeeFinancialAccount(payeeFinancialAccount) };
        }
    }

    public class PaymentTerm
    {
        [JsonProperty("Note", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "Note", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> Note { get; set; }

        public PaymentTerm(){}
        public PaymentTerm(string note)
        {
            Note = new List<TextValue> { new TextValue(note) };
        }
    }

    public class PostalAddress
    {
        [JsonProperty("CityName", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "CityName", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> CityName { get; set; }

        [JsonProperty("PostalZone", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "PostalZone", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> PostalZone { get; set; }

        [JsonProperty("CountrySubentityCode", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "CountrySubentityCode", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> CountrySubentityCode { get; set; }

        [JsonProperty("AddressLine", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "AddressLine", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<AddressLine> AddressLine { get; set; }

        [JsonProperty("Country", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "Country", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<Country> Country { get; set; }

        public PostalAddress(){}
    }

    public class PrepaidPayment
    {
        [JsonProperty("ID", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "ID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> ID { get; set; }

        [JsonProperty("PaidAmount", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "PaidAmount", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<Amount> PaidAmount { get; set; }

        [JsonProperty("PaidDate", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "PaidDate", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> PaidDate { get; set; }

        [JsonProperty("PaidTime", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "PaidTime", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> PaidTime { get; set; }

        public PrepaidPayment(){}
    }

    public class Price
    {
        [JsonProperty("PriceAmount", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "PriceAmount", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<Amount> PriceAmount { get; set; }

        public Price(){}

        public Price(decimal priceAmount, string currencyID)
        {
            PriceAmount = new List<Amount> { new Amount(priceAmount, currencyID) };
        }
    }


    public class Shipment
    {
        [JsonProperty("ID", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "ID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> ID { get; set; }

        [JsonProperty("FreightAllowanceCharge", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "FreightAllowanceCharge", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<FreightAllowanceCharge> FreightAllowanceCharge { get; set; }

        public Shipment(){}

    }

    public class TaxCategory
    {
        [JsonProperty("ID", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "ID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> ID { get; set; }

        [JsonProperty("TaxExemptionReason", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "TaxExemptionReason", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<TextValue> TaxExemptionReason { get; set; }

        [JsonProperty("TaxScheme", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "TaxScheme", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<TaxScheme> TaxScheme { get; set; }

        public TaxCategory(){}
    }

    public class TaxScheme
    {
        [JsonProperty("ID", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "ID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<TaxSchemeId> ID { get; set; }

        public TaxScheme(){}

        public TaxScheme(string value, string schemeID, string schemeAgencyID)
        {
            ID = new List<TaxSchemeId> { new TaxSchemeId(value, schemeID, schemeAgencyID) };
        }
    }

    public class TaxSubtotal
    {
        [JsonProperty("TaxableAmount", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "TaxableAmount", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<Amount> TaxableAmount { get; set; }

        [JsonProperty("TaxAmount", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "TaxAmount", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<Amount> TaxAmount { get; set; }

        [JsonProperty("Percent", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "Percent", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<NumValue> Percent { get; set; }

        [JsonProperty("TaxCategory", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "TaxCategory", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<TaxCategory> TaxCategory { get; set; }

        public TaxSubtotal(){}
    }

    public class TaxTotal
    {
        [JsonProperty("TaxAmount", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "TaxAmount", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public List<Amount> TaxAmount { get; set; }

        [JsonProperty("TaxSubtotal", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "TaxSubtotal", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<TaxSubtotal> TaxSubtotal { get; set; }

        public TaxTotal(){}
    }

    // Class untuk menyimpan nilai teks
    public class TextValue
    {
        [JsonProperty("_", NullValueHandling = NullValueHandling.Ignore)]
        [XmlText]
        public string Value { get; set; }

        public TextValue() { }

        public TextValue(string value)
        {
            Value = value;
        }
    }

    // Class untuk menyimpan nilai numerik
    public class NumValue
    {
        [JsonProperty("_", NullValueHandling = NullValueHandling.Ignore)]
        [XmlText]
        public decimal Value { get; set; }

        public NumValue() { }

        public NumValue(decimal numValue)
        {
            Value = numValue;
        }
    }

    // Class untuk menyimpan nilai boolean
    public class BoolValue
    {
        [JsonProperty("_", NullValueHandling = NullValueHandling.Ignore)]
        [XmlText]
        public Boolean Value { get; set; }

        public BoolValue() { }

        public BoolValue(Boolean boolValue)
        {
            Value = boolValue;
        }
    }
}
