using System.Text.Json.Serialization;

namespace MyInvois.ApiClient.Models
{
    public class SubmitDocumentRequest
    {
        [JsonPropertyName("documents")]
        public OneDocument[] Documents { get; set; }
    }

    public class OneDocument
    {
        [JsonPropertyName("format")]
        public string Format { get; set; }

        [JsonPropertyName("documentHash")]
        public string DocumentHash { get; set; }

        [JsonPropertyName("codeNumber")]
        public string CodeNumber { get; set; }

        [JsonPropertyName("document")]
        public string Document { get; set; }

    }
}