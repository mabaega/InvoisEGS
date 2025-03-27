using System.Xml.Serialization;

namespace MyInvois.ApiClient.Models.EInvoice;
[XmlRoot(ElementName = "InvoicedQuantity", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
public class InvoicedQuantity
{

    [XmlAttribute(AttributeName = "unitCode")]
    public string UnitCode { get; set; }

    [XmlText]
    public string Quantity { get; set; }

    [XmlIgnore]
    public decimal NumericValue
    {
        get => decimal.TryParse(Quantity, out decimal result) ? result : 0;
        set => Quantity = value.ToString();
    }

    public InvoicedQuantity(string unitCode, double quantity)
    {
        UnitCode = unitCode;
        Quantity = quantity.ToString();
    }
    public InvoicedQuantity() { }
}

