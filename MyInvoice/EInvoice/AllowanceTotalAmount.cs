using System.Xml.Serialization;

namespace InvoisEGS.ApiClient.Models.EInvoice;
[XmlRoot(ElementName = "AllowanceTotalAmount", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
public class AllowanceTotalAmount
{

    [XmlAttribute(AttributeName = "currencyID")]
    public string CurrencyID { get; set; }

    [XmlText]
    public double Text { get; set; }
}

