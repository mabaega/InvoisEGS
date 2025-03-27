using System.Xml.Serialization;

namespace InvoisEGS.ApiClient.Models.EInvoice;
[XmlRoot(ElementName = "AddressLine", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
public class AddressLine
{

    [XmlElement(ElementName = "Line", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
    public string Line { get; set; }
}

