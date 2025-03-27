using Newtonsoft.Json;

namespace InvoisEGS.ApiClient.ApiModels
{
    public class SubmitDocumentRequest
    {
        [JsonProperty("documents")]
        public OneDocument[] Documents { get; set; }
    }

    public class OneDocument
    {
        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("documentHash")]
        public string DocumentHash { get; set; }

        [JsonProperty("codeNumber")]
        public string CodeNumber { get; set; }

        [JsonProperty("document")]
        public string Document { get; set; }
    }
}