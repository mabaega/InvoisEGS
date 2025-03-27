using System.Text.Json.Serialization;

namespace MyInvois.ApiClient.Models
{
    public class TokenInfo
    {
        private static string TokenDirectory =>
            Environment.GetEnvironmentVariable("HOME") != null
                ? Path.Combine(Environment.GetEnvironmentVariable("HOME"), "Data", "Tokens") // Azure
                : Path.Combine(AppContext.BaseDirectory, "Data", "Tokens"); // Local

        public string AccessToken { get; set; } // Changed to set for serialization
        public DateTime ExpiresAt { get; set; }
        public string TokenType { get; set; }

        public bool IsValid => DateTime.UtcNow < ExpiresAt.AddMinutes(-5);

        public TokenInfo() { } // Empty constructor for deserialization

        public TokenInfo(TokenResponse response)
        {
            AccessToken = response.AccessToken;
            TokenType = response.TokenType;
            ExpiresAt = DateTime.UtcNow.AddSeconds(response.ExpiresIn);
        }

        public static void SaveToken(string clientId, TokenInfo token)
        {
            Directory.CreateDirectory(TokenDirectory);
            string filePath = Path.Combine(TokenDirectory, $"{clientId}.json");
            string json = System.Text.Json.JsonSerializer.Serialize(token);
            File.WriteAllText(filePath, json);
        }

        public static TokenInfo LoadToken(string clientId)
        {
            string filePath = Path.Combine(TokenDirectory, $"{clientId}.json");
            if (!File.Exists(filePath))
            {
                return null;
            }

            string json = File.ReadAllText(filePath);
            TokenInfo token = System.Text.Json.JsonSerializer.Deserialize<TokenInfo>(json);
            return token?.IsValid == true ? token : null;
        }
    }

    public class TokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }
    }
}