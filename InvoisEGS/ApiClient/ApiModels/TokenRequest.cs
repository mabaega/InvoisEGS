using Newtonsoft.Json;

namespace InvoisEGS.ApiClient.ApiModels
{
    public class TokenRequest
    {
        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("client_secret")]
        public string ClientSecret { get; set; }

        [JsonProperty("grant_type")]
        public string GrantType { get; set; } = "client_credentials";

        [JsonProperty("scope")]
        public string Scope { get; set; } = "InvoicingAPI";

        [JsonIgnore]
        public string IntegrationType { get; set; }
    }
}
