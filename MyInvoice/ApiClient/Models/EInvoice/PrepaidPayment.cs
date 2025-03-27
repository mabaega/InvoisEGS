using System.Globalization;
using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace MyInvois.ApiClient.Models.EInvoice;
[XmlRoot(ElementName = "PrepaidPayment", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
public class PrepaidPayment
{

    [XmlElement(ElementName = "ID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
    public ID ID { get; set; }

    [XmlElement(ElementName = "PaidAmount", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
    public Amount PaidAmount { get; set; }

    [XmlElement(ElementName = "PaidDate", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
    public string PaidDate { get; set; }

    [XmlIgnore]
    [JsonIgnore]
    public DateTime PaidDateAsDateTime
    {
        get => DateTime.TryParseExact(PaidDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result) ? result : DateTime.MinValue;
        set => PaidDate = value.ToString("yyyy-MM-dd");
    }

    [XmlElement(ElementName = "PaidTime", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
    public string PaidTime { get; set; }

    [XmlIgnore]
    [JsonIgnore]
    public DateTime PaidTimeAsDateTime
    {
        get => DateTime.TryParseExact(PaidTime, "HH:mm:ssZ", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result) ? result : DateTime.MinValue;
        set => PaidTime = value.ToString("HH:mm:ssZ");
    }
}

