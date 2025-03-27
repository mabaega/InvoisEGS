using System.Text.Json.Serialization;

namespace MyInvois.ApiClient.Models
{
    public class TaxpayerInfo
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("tin")]
        public string Tin { get; set; }

        [JsonPropertyName("registrationStatus")]
        public string RegistrationStatus { get; set; }
    }
}