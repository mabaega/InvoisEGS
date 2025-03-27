using System.Text.Json.Serialization;

namespace MyInvois.ApiClient.Models
{
    public class TaxpayerValidationResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("isValid")]
        public bool IsValid { get; set; }

        [JsonPropertyName("taxpayerInfo")]
        public TaxpayerInfo TaxpayerInfo { get; set; }
    }
}