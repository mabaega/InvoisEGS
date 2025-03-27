using System.Xml.Serialization;

namespace InvoisEGS.ApiClient.Models.EInvoice;
[XmlRoot(ElementName = "ID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
public class ID
{
    [XmlAttribute(AttributeName = "schemeID")]
    public string? SchemeID { get; set; }

    [XmlAttribute(AttributeName = "schemeAgencyID")]
    public string? SchemeAgencyID { get; set; }

    [XmlText]
    public string Text { get; set; }

    public ID(string text, string? schemeID, string? schemeAgencyID)
    {
        if (!string.IsNullOrEmpty(SchemeID)) { SchemeID = schemeID; }
        if (!string.IsNullOrEmpty(SchemeAgencyID)) { SchemeAgencyID = schemeAgencyID; }
        Text = text;
    }

    public ID(string text)
    {
        Text = text;
    }

    public ID()
    {
    }
}

