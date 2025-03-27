using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;

public class Program
{
    public static void Main()
    {
        // JSON Input
        string jsonData = @"
        {
            ""Party"": {
                ""PartyIdentification"": [
                    {
                        ""ID"": [
                            {
                                ""schemeID"": ""TIN"",
                                ""_"": ""Supplier's TIN""
                            }
                        ]
                    },
                    {
                        ""ID"": [
                            {
                                ""schemeID"": ""BRN"",
                                ""_"": ""Supplier's BRN""
                            }
                        ]
                    }
                ]
            }
        }";

        Console.WriteLine("=== JSON to XML ===");
        var party = JsonConvert.DeserializeObject<PartyContainer>(jsonData);
        string xmlOutput = SerializeToXml(party);
        Console.WriteLine(xmlOutput);

        Console.WriteLine("\n=== XML to JSON ===");
        var deserializedParty = DeserializeFromXml(xmlOutput);
        string jsonOutput = JsonConvert.SerializeObject(deserializedParty, Formatting.Indented);
        Console.WriteLine(jsonOutput);
    }

    // Serialize ke XML
    public static string SerializeToXml(PartyContainer partyContainer)
    {
        XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
        ns.Add("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
        ns.Add("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");

        var serializer = new XmlSerializer(typeof(PartyContainer));
        using (StringWriter writer = new StringWriter())
        {
            serializer.Serialize(writer, partyContainer, ns);
            return writer.ToString();
        }
    }

    // Deserialize dari XML
    public static PartyContainer DeserializeFromXml(string xmlData)
    {
        var serializer = new XmlSerializer(typeof(PartyContainer));
        using (StringReader reader = new StringReader(xmlData))
        {
            return (PartyContainer)serializer.Deserialize(reader);
        }
    }
}

// ==== CLASS UNTUK XML & JSON ====

[XmlRoot("Party", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
public class Party
{
    [XmlElement("PartyIdentification", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
    [JsonProperty("PartyIdentification")]
    public List<PartyIdentification> PartyIdentification { get; set; }
}

public class PartyContainer
{
    [XmlElement("Party", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
    [JsonProperty("Party")]
    public Party Party { get; set; }
}

public class PartyIdentification
{
    [XmlElement("ID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
    public List<IdentificationID> ID { get; set; }
}

public class IdentificationID
{
    [XmlAttribute("schemeID")]
    [JsonProperty("schemeID")]
    public string SchemeID { get; set; }

    [XmlText]
    [JsonProperty("_")]
    public string Value { get; set; }
}
