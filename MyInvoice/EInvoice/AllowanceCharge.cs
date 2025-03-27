using System.Globalization;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace InvoisEGS.ApiClient.Models.EInvoice;
[XmlRoot(ElementName = "AllowanceCharge", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
public class AllowanceCharge
{

    [XmlElement(ElementName = "ChargeIndicator", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
    public bool ChargeIndicator { get; set; }

    [XmlElement(ElementName = "AllowanceChargeReason", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
    public string AllowanceChargeReason { get; set; }

    [XmlElement(ElementName = "Amount", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
    public Amount Amount { get; set; }

    [XmlElement(ElementName = "MultiplierFactorNumeric", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
    public string? MultiplierFactorNumeric { get; set; }

    [XmlElement(ElementName = "TaxCategory", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
    public TaxCategory[] TaxCategory { get; set; }

    [XmlIgnore]
    public decimal MultiplierFactorNumericValue
    {
        get => decimal.TryParse(MultiplierFactorNumeric, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal result) ? result : 0;
        set => MultiplierFactorNumeric = value.ToString(CultureInfo.InvariantCulture);
    }
}

