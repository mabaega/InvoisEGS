using Newtonsoft.Json;

namespace InvoisEGS.ApiClient.ApiModels
{
    public class TokenInfo
    {
        private static string TokenDirectory =>
            Environment.GetEnvironmentVariable("HOME") != null
                ? Path.Combine(Environment.GetEnvironmentVariable("HOME"), "Data", "Tokens")
                : Path.Combine(AppContext.BaseDirectory, "Data", "Tokens");

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        public DateTime ExpiresAt { get; set; }

        public bool IsValid
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(AccessToken))
                        return false;

                    var now = DateTime.UtcNow;
                    var buffer = TimeSpan.FromMinutes(5);
                    return now < ExpiresAt - buffer;
                }
                catch
                {
                    return false;
                }
            }
        }

        public TokenInfo() { }

        public TokenInfo(TokenResponse response)
        {
            if (response == null)
                throw new ArgumentException("Token response is null");

            AccessToken = response.AccessToken;
            TokenType = response.TokenType;
            ExpiresIn = response.ExpiresIn;
            ExpiresAt = DateTime.UtcNow.AddSeconds(response.ExpiresIn);
        }

        public static void SaveToken(string clientId, TokenInfo token)
        {
            if (string.IsNullOrEmpty(clientId) || token == null || !token.IsValid)
                return;

            try
            {
                Directory.CreateDirectory(TokenDirectory);
                string filePath = Path.Combine(TokenDirectory, $"{clientId}.json");
                string json = JsonConvert.SerializeObject(token);
                File.WriteAllText(filePath, json);
            }
            catch
            {
                // Fail silently - token will be regenerated next time
            }
        }

        public static TokenInfo? LoadToken(string clientId)
        {
            try
            {
                string filePath = Path.Combine(TokenDirectory, $"{clientId}.json");
                if (!File.Exists(filePath))
                    return null;

                string json = File.ReadAllText(filePath);
                var token = JsonConvert.DeserializeObject<TokenInfo>(json);
                return token?.IsValid == true ? token : null;
            }
            catch
            {
                return null;
            }
        }
    }

    public class TokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }
    }
}