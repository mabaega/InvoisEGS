
using MyInvois.ApiClient.Helpers;
using MyInvois.ApiClient.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MyInvois.ApiClient.Services
{
    public class ApiHelper : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private TokenInfo _currentToken;
        private readonly string _clientId;
        private readonly string _clientSecret;

        public ApiHelper(string clientId, string clientSecret, string baseUrl = "https://preprod-api.myinvois.hasil.gov.my")
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _baseUrl = baseUrl;
            _httpClient = new HttpClient();

            // Try to load existing token using clientId
            _currentToken = TokenStore.LoadToken(_clientId);
            if (_currentToken?.IsValid == true)
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _currentToken.AccessToken);
            }
        }

        public async Task<TokenInfo> GetAccessTokenAsync()
        {
            if (_currentToken?.IsValid == true)
            {
                return _currentToken;
            }

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
            HttpResponseMessage response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                ErrorResponse error = await ErrorResponse.HandleResponseAsync(response);
                throw new HttpRequestException(
                    $"Authentication failed: {error?.Error?.ErrorMessage}",
                    null,
                    response.StatusCode
                );
            }

            TokenResponse tokenResponse = JsonSerializer.Deserialize<TokenResponse>(
                await response.Content.ReadAsStringAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            _currentToken = new TokenInfo(tokenResponse);
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _currentToken.AccessToken);

            // Save token using clientId
            TokenStore.SaveToken(_clientId, _currentToken);

            return _currentToken;
        }

        //Submit Documents, to submit document to server
        public async Task<SubmitDocumentResponse> SubmitDocumentAsync(SubmitDocumentRequest submitRequest)
        {
            // Ensure we have a valid token
            if (_currentToken?.IsValid != true)
            {
                await GetAccessTokenAsync();
            }

            HttpRequestMessage request = new(HttpMethod.Post, $"{_baseUrl}/api/v1.0/documentsubmissions");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            StringContent content = new(
                JsonSerializer.Serialize(submitRequest),
                System.Text.Encoding.UTF8,
                "application/json"
            );
            request.Content = content;

            HttpResponseMessage response = await _httpClient.SendAsync(request);

            // Handle 202 Accepted status specifically
            if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                return JsonSerializer.Deserialize<SubmitDocumentResponse>(
                    await response.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );
            }

            // Handle specific error cases
            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                ErrorInfo error = JsonSerializer.Deserialize<ErrorInfo>(errorContent);

                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.BadRequest when error?.Code == ErrorInfo.ErrorCodes.BadStructure:
                        throw new InvalidOperationException("Invalid document structure: " + error.Message);

                    case System.Net.HttpStatusCode.BadRequest when error?.Code == ErrorInfo.ErrorCodes.MaximumSizeExceeded:
                        throw new InvalidOperationException("Document size exceeds limit: " + error.Message);

                    case System.Net.HttpStatusCode.Forbidden when error?.Code == ErrorInfo.ErrorCodes.IncorrectSubmitter:
                        throw new UnauthorizedAccessException("Unauthorized submitter: " + error.Message);

                    case System.Net.HttpStatusCode.UnprocessableEntity when error?.Code == ErrorInfo.ErrorCodes.DuplicateSubmission:
                        // Get retry delay from header
                        if (response.Headers.TryGetValues("Retry-After", out IEnumerable<string> retryValues) &&
                            int.TryParse(retryValues.FirstOrDefault(), out int retrySeconds))
                        {
                            throw new InvalidOperationException($"Duplicate submission. Retry after {retrySeconds} seconds");
                        }
                        throw new InvalidOperationException("Duplicate submission detected");

                    default:
                        throw new HttpRequestException(
                            $"Document submission failed: {error?.Message}",
                            null,
                            response.StatusCode
                        );
                }
            }

            // Should not reach here as 202 is the only success case
            throw new InvalidOperationException("Unexpected response from server");
        }

        //Documet Submission to get Document status
        private readonly SemaphoreSlim _requestThrottler = new(1, 1);
        private DateTime _lastRequestTime = DateTime.MinValue;
        private bool _disposed;

        public async Task<(SubmitDocumentResponse Submit, DocumentStatusResponse Status)> SubmitAndMonitorAsync(
            SubmitDocumentRequest submitRequest,
            CancellationToken cancellationToken = default)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(ApiHelper));
            }

            // Submit document first
            SubmitDocumentResponse submitResponse = await SubmitDocumentAsync(submitRequest);

            // Wait initial 3 seconds before first status check
            await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken);

            // Monitor status until completion
            DocumentStatusResponse finalStatus = null;
            while (!cancellationToken.IsCancellationRequested)
            {
                await _requestThrottler.WaitAsync(cancellationToken);
                try
                {
                    TimeSpan timeSinceLastRequest = DateTime.UtcNow - _lastRequestTime;
                    if (timeSinceLastRequest < TimeSpan.FromSeconds(3))
                    {
                        await Task.Delay(TimeSpan.FromSeconds(3) - timeSinceLastRequest, cancellationToken);
                    }

                    _lastRequestTime = DateTime.UtcNow;
                    finalStatus = await GetDocumentStatusAsync(submitResponse.SubmissionUID);

                    if (finalStatus.OverallStatus is "Completed" or "Rejected" or "Failed")
                    {
                        break;
                    }
                }
                finally
                {
                    _requestThrottler.Release();
                }
            }

            return (submitResponse, finalStatus);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _requestThrottler.Dispose();
                _httpClient.Dispose();
                _disposed = true;
            }
        }

        public async Task<DocumentStatusResponse> PollDocumentStatusAsync(
    string submissionUid,
    CancellationToken cancellationToken = default,
    int pageNo = 1,
    int pageSize = 10)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(ApiHelper));
            }

            await _requestThrottler.WaitAsync(cancellationToken);  // Changed from _throttler
            try
            {
                TimeSpan timeSinceLastRequest = DateTime.UtcNow - _lastRequestTime;
                if (timeSinceLastRequest < TimeSpan.FromSeconds(3))
                {
                    await Task.Delay(TimeSpan.FromSeconds(3) - timeSinceLastRequest, cancellationToken);
                }

                _lastRequestTime = DateTime.UtcNow;
                return await GetDocumentStatusAsync(submissionUid, pageNo, pageSize);
            }
            finally
            {
                _requestThrottler.Release();  // Changed from _throttler
            }
        }

        public async IAsyncEnumerable<DocumentStatusResponse> MonitorDocumentStatusAsync(
            string submissionUid,
            CancellationToken cancellationToken = default)  // Added attribute
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                DocumentStatusResponse status = await PollDocumentStatusAsync(submissionUid, cancellationToken);
                yield return status;

                if (status.OverallStatus is "Completed" or "Rejected" or "Failed")
                {
                    yield break;
                }
            }
        }
        public async Task<DocumentStatusResponse> GetDocumentStatusAsync(string submissionUid, int pageNo = 1, int pageSize = 10)
        {
            // Ensure we have a valid token
            if (_currentToken?.IsValid != true)
            {
                await GetAccessTokenAsync();
            }

            HttpRequestMessage request = new(
                HttpMethod.Get,
                $"{_baseUrl}/api/v1.0/documentsubmissions/{submissionUid}?pageNo={pageNo}&pageSize={pageSize}"
            );
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await _httpClient.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonSerializer.Deserialize<DocumentStatusResponse>(
                    await response.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );
            }

            ErrorResponse error = await ErrorResponse.HandleResponseAsync(response);
            throw new HttpRequestException(
                $"Failed to get document status: {error?.Error?.ErrorMessage}",
                null,
                response.StatusCode
            );
        }
    }
}