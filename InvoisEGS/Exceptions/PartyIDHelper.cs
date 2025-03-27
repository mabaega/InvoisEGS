using InvoisEGS.ApiClient.ApiHelpers;
using InvoisEGS.ApiClient.ApiModels;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace InvoisEGS.Exceptions
{
    public class PartyIDHelper
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly string _userTIN;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _cachePath;
        private TokenInfo _currentToken;
        private List<PartyIDValidation> _cachedValidations;

        public PartyIDHelper(HttpClient httpClient, string userTIN, string clientId, string clientSecret, IntegrationType integrationType)
        {
            _httpClient = httpClient;
            _userTIN = userTIN;
            _clientId = clientId;
            _clientSecret = clientSecret;
            _baseUrl = integrationType.GetApiBaseUrl();
            _currentToken = new TokenInfo();

            string baseDirectory = Environment.GetEnvironmentVariable("HOME") != null
                ? Path.Combine(Environment.GetEnvironmentVariable("HOME") ?? string.Empty, "Data", "TaxPayerTin") // Azure
                : Path.Combine(AppContext.BaseDirectory, "Data", "TaxPayerTin"); // Local

            // Ensure directory exists
            if (!Directory.Exists(baseDirectory))
            {
                Directory.CreateDirectory(baseDirectory);
            }

            _cachePath = Path.Combine(baseDirectory, $"{_userTIN}.json");
            _cachedValidations = LoadCache();
        }

        private List<PartyIDValidation> LoadCache()
        {
            try
            {
                if (File.Exists(_cachePath))
                {
                    string jsonContent = File.ReadAllText(_cachePath);
                    var cache = JsonConvert.DeserializeObject<List<PartyIDValidation>>(jsonContent)
                        ?? new List<PartyIDValidation>();

                    // Clean expired entries
                    return cache.Where(x => DateTime.UtcNow.Subtract(x.ValidatedAt).TotalDays < 30).ToList();
                }
            }
            catch (Exception)
            {
                // If there's any error reading the cache, start fresh
            }
            return new List<PartyIDValidation>();
        }

        private void SaveCache()
        {
            try
            {
                if (_cachedValidations == null || !_cachedValidations.Any())
                {
                    // Console.WriteLine("No validations to save");
                    return;
                }

                string directoryPath = Path.GetDirectoryName(_cachePath);
                if (string.IsNullOrEmpty(directoryPath))
                {
                    // Console.WriteLine($"Invalid directory path for cache: {_cachePath}");
                    return;
                }

                Directory.CreateDirectory(directoryPath);

                using (var fileStream = new FileStream(_cachePath, FileMode.Create, FileAccess.Write))
                using (var streamWriter = new StreamWriter(fileStream))
                using (var jsonWriter = new JsonTextWriter(streamWriter))
                {
                    var serializer = new JsonSerializer { Formatting = Formatting.Indented };
                    serializer.Serialize(jsonWriter, _cachedValidations);
                }

                // Console.WriteLine($"Cache saved to: {_cachePath}");
                // Console.WriteLine($"Saved {_cachedValidations.Count} validations");
            }
            catch (Exception ex)
            {
                // Console.WriteLine($"Failed to save cache: {ex.Message}");
                // Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        public async Task<PartyIDValidation> GetPartyID(string idType, string idValue, string partyTIN)
        {
            // Console.WriteLine($"GetPartyID called with idType: {idType}, idValue: {idValue}, partyTIN: {partyTIN}");
            
            // Clean and check cache
            var cachedValidation = _cachedValidations
                .FirstOrDefault(x => x.IdType == idType &&
                                    x.IdValue == idValue &&
                                    x.PartyTIN == partyTIN &&
                                    DateTime.UtcNow.Subtract(x.ValidatedAt).TotalDays < 30);

            if (cachedValidation != null)
            {
                // Console.WriteLine("Found in cache, returning cached validation");
                return cachedValidation;
            }

            try
            {
                await GetAccessTokenAsync();
                var requestUrl = $"{_baseUrl}/api/v1.0/taxpayer/validate/{partyTIN}?idType={idType}&idValue={idValue}";
                // Console.WriteLine($"Making API request to: {requestUrl}");

                var response = await _httpClient.GetAsync(requestUrl);
                var responseContent = await response.Content.ReadAsStringAsync();
                // Console.WriteLine($"API Response: {response.StatusCode} - {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    // Console.WriteLine("API call successful, creating validation");
                    var validation = new PartyIDValidation
                    {
                        IdType = idType,
                        IdValue = idValue,
                        PartyTIN = partyTIN,
                        ValidatedAt = DateTime.UtcNow
                    };

                    // Console.WriteLine($"Adding validation to cache: {JsonConvert.SerializeObject(validation)}");
                    _cachedValidations.Add(validation);
                    
                    // Console.WriteLine("Calling SaveCache");
                    SaveCache();
                    
                    // Console.WriteLine("Returning validation");
                    return validation;
                }

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    // Console.WriteLine($"ID not found: {idType} - {idValue}");
                    return null;
                }

                HandleErrorResponse(response, responseContent);
            }
            catch (Exception ex)
            {
                // Console.WriteLine($"Validation Error: {ex.Message}");
                throw new InvalidOperationException($"Failed to validate {idType}: {ex.Message}", ex);
            }

            return null;
        }

        private void HandleErrorResponse(HttpResponseMessage response, string responseContent)
        {
            // Console.WriteLine($"Error Response: {response.StatusCode} - {responseContent}");
            var errorObject = JsonConvert.DeserializeObject<dynamic>(responseContent);
            string errorMessage = errorObject?.error?.message?.ToString()
                ?? errorObject?.error?.ToString()
                ?? errorObject?.ToString()
                ?? responseContent;

            throw new HttpRequestException(
                $"Request failed: {errorMessage}",
                null,
                response.StatusCode
            );
        }

         private async Task<TokenInfo> GetAccessTokenAsync()
        {
            try
            {
                // Try to load token from storage first
                var storedToken = TokenInfo.LoadToken(_clientId);
                if (storedToken?.IsValid == true)
                {
                    // Console.WriteLine($"Using stored token - Expires at: {storedToken.ExpiresAt}");
                    _currentToken = storedToken;
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", _currentToken.AccessToken);
                    return _currentToken;
                }

                // Check if current token is still valid
                if (_currentToken?.IsValid == true)
                {
                    // Console.WriteLine($"Using current token - Expires at: {_currentToken.ExpiresAt}");
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", _currentToken.AccessToken);
                    return _currentToken;
                }

                // Get new token
                HttpRequestMessage request = new(HttpMethod.Post, $"{_baseUrl}/connect/token");
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                FormUrlEncodedContent content = new(new[]
                {
                    new KeyValuePair<string, string>("client_id", _clientId),
                    new KeyValuePair<string, string>("client_secret", _clientSecret),
                    new KeyValuePair<string, string>("grant_type", "client_credentials"),
                    new KeyValuePair<string, string>("scope", "InvoicingAPI")
                });

                request.Content = content;
                // Console.WriteLine($"Requesting new token from: {request.RequestUri}");

                HttpResponseMessage response = await _httpClient.SendAsync(request);
                string responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    HandleErrorResponse(response, responseContent);
                }

                TokenResponse tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent);
                _currentToken = new TokenInfo(tokenResponse);

                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _currentToken.AccessToken);

                TokenStore.SaveToken(_clientId, _currentToken);
                // Console.WriteLine($"New token generated - Valid until: {_currentToken.ExpiresAt}");

                return _currentToken;
            }
            catch (Exception ex)
            {
                // Console.WriteLine($"Token generation failed: {ex.Message}");
                throw;
            }
        }
    }
}

