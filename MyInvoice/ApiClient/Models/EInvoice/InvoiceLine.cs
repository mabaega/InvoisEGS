using System.Xml.Serialization;

namespace MyInvois.ApiClient.Models.EInvoice;
[XmlRoot(ElementName = "InvoiceLine", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
public class InvoiceLine
{

    [XmlElement(ElementName = "ID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
    public ID ID { get; set; }

    [XmlElement(ElementName = "InvoicedQuantity", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
    public InvoicedQuantity InvoicedQuantity { get; set; }

    [XmlElement(ElementName = "LineExtensionAmount", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
    public Amount LineExtensionAmount { get; set; }

    [XmlElement(ElementName = "AllowanceCharge", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
    public AllowanceCharge[] AllowanceCharge { get; set; }

    [XmlElement(ElementName = "TaxTotal", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
    public TaxTotal TaxTotal { get; set; }

    [XmlElement(ElementName = "Item", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
    public Item Item { get; set; }

    [XmlElement(ElementName = "Price", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
    public Price Price { get; set; }

    [XmlElement(ElementName = "ItemPriceExtension", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
    public ItemPriceExtension ItemPriceExtension { get; set; }
}

