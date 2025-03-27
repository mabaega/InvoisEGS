using Newtonsoft.Json;

namespace InvoisEGS.Exceptions
{
    public class PartyIDValidation
    {
        [JsonProperty("partyTIN")]
        public string PartyTIN { get; set; }

        [JsonProperty("idType")]
        public string IdType { get; set; }

        [JsonProperty("idValue")]
        public string IdValue { get; set; }

        [JsonProperty("validatedAt")]
        public DateTime ValidatedAt { get; set; }
    }
}

