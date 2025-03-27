using System.Globalization;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

[XmlRoot(ElementName = "InvoicePeriod", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
public class InvoicePeriod
{

    [XmlElement(ElementName = "StartDate", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
    public string StartDate { get; set; }

    [XmlIgnore]
    [JsonIgnore]
    public DateTime StartDateAsDateTime
    {
        get => DateTime.TryParseExact(StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result) ? result : DateTime.MinValue;
        set => StartDate = value.ToString("yyyy-MM-dd");
    }

    [XmlElement(ElementName = "EndDate", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
    public string EndDate { get; set; }

    [XmlIgnore]
    [JsonIgnore]
    public DateTime EndDateAsDateTime
    {
        get => DateTime.TryParseExact(EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result) ? result : DateTime.MinValue;
        set => EndDate = value.ToString("yyyy-MM-dd");
    }

    [XmlElement(ElementName = "Description", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
    public string Description { get; set; }
}

