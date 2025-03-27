using System.Xml.Serialization;

namespace InvoisEGS.ApiClient.Models.EInvoice;

[XmlRoot(ElementName = "InvoiceTypeCode", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
public class InvoiceTypeCode
{

    [XmlAttribute(AttributeName = "listVersionID")]
    public string ListVersionID { get; set; }

    [XmlText]
    public string Text { get; set; }

    public static implicit operator string(InvoiceTypeCode code)
    {
        return code?.Text;
    }

    public static implicit operator InvoiceTypeCode(string code)
    {
        return new InvoiceTypeCode { Text = code };
    }
}

