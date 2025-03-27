using MyInvois.ApiClient.Models;
using System.Text.Json;

namespace MyInvois.ApiClient.Helpers
{
    public class TokenStore
    {
        private static readonly string TokenPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "MyInvois",
            "tokens"
        );

        public static void SaveToken(string clientId, TokenInfo token)
        {
            Directory.CreateDirectory(TokenPath);
            string filePath = Path.Combine(TokenPath, $"{clientId}.json");
            string json = JsonSerializer.Serialize(token);
            File.WriteAllText(filePath, json);
        }

        public static TokenInfo LoadToken(string clientId)
        {
            string filePath = Path.Combine(TokenPath, $"{clientId}.json");
            if (!File.Exists(filePath))
            {
                return null;
            }

            string json = File.ReadAllText(filePath);
            TokenInfo token = JsonSerializer.Deserialize<TokenInfo>(json);
            return token?.IsValid == true ? token : null;
        }
    }
}