using System.Globalization;
using System.Xml.Serialization;

namespace InvoisEGS.ApiClient.Models.EInvoice;
[XmlRoot(ElementName = "InvoicedQuantity", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
public class InvoicedQuantity
{

    [XmlAttribute(AttributeName = "unitCode")]
    public string UnitCode { get; set; }

    [XmlText]
    public string Text { get; set; }

    [XmlIgnore]
    public decimal NumericValue
    {
        get => decimal.TryParse(Text, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal result) ? result : 0;
        set => Text = value.ToString(CultureInfo.InvariantCulture);
    }

    public InvoicedQuantity(string unitCode, decimal quantity)
    {
        UnitCode = unitCode;
        Text = quantity.ToString(CultureInfo.InvariantCulture);
    }
    public InvoicedQuantity() { }
}

