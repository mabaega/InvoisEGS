using System.Globalization;
using System.Xml.Serialization;

namespace MyInvois.ApiClient.Models.EInvoice;
[XmlRoot(ElementName = "Amount", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
public class Amount
{

    [XmlAttribute(AttributeName = "currencyID")]
    public string CurrencyID { get; set; }

    [XmlText]
    public string Text { get; set; }

    [XmlIgnore]
    public decimal NumericValue
    {
        get => decimal.TryParse(Text, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal result) ? result : 0;
        set => Text = value.ToString(CultureInfo.InvariantCulture);
    }
}

