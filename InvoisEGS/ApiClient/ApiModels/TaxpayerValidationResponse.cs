using Newtonsoft.Json;

namespace InvoisEGS.ApiClient.ApiModels
{
    public class TaxpayerValidationResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("isValid")]
        public bool IsValid { get; set; }

        [JsonProperty("taxpayerInfo")]
        public TaxpayerInfo TaxpayerInfo { get; set; }
    }
}