
using Newtonsoft.Json;

namespace InvoisEGS.ApiClient.ApiModels
{
    public class TaxpayerInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("tin")]
        public string Tin { get; set; }

        [JsonProperty("registrationStatus")]
        public string RegistrationStatus { get; set; }
    }
}