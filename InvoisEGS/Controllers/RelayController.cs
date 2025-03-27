using InvoisEGS.ApiClient.ApiModels;
using InvoisEGS.ApiClient.XModels;
using InvoisEGS.Exceptions;
using InvoisEGS.Models;
using InvoisEGS.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net;
using InvoisEGS.ApiClient;
using InvoisEGS.ApiClient.ApiHelpers;

namespace InvoisEGS.Controllers
{
    public class RelayController : Controller
    {
        private readonly ILogger<RelayController> _logger;
        private readonly HttpClient _httpClient;
        private TokenInfo _currentToken;
        private string _clientId;
        private string _clientSecret;
        private string _baseUrl;

        public RelayController(ILogger<RelayController> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
            _currentToken = new TokenInfo();
            _clientId = string.Empty;
            _clientSecret = string.Empty;
            _baseUrl = string.Empty;
        }

        [HttpPost("relay")]
        public async Task<IActionResult> ReceiveData([FromForm] Dictionary<string, string> formData, CancellationToken cancellationToken)
        {
            try
            {
                if (formData == null || formData.Count == 0)
                {
                    return View("Error", new ErrorViewModel
                    {
                        ErrorMessage = "No data was received. Please try submitting the form again.",
                        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                    });
                }

                string Data = Utils.GetValue(formData, "Data");
                string BusinessDetailJson = RelayDataHelper.GetValueJson(Data, "BusinessDetails");

                (ApplicationConfig AppConfig, long egsversion) = RelayDataHelper.InitializeBusinessData(BusinessDetailJson);

                if (string.IsNullOrEmpty(AppConfig?.ClientID))
                {
                    SetupViewModel setupViewModel = new()
                    {
                        Referrer = Utils.GetValue(formData, "Referrer"),
                        Api = Utils.GetValue(formData, "Api"),
                        Token = Utils.GetValue(formData, "Token"),
                        BusinessDetailJson = BusinessDetailJson,
                        EGSVersion = egsversion,
                    };

                    TempData["SetupViewModel"] = JsonConvert.SerializeObject(setupViewModel);
                    return RedirectToAction("Register", "Setup");
                }

                RelayData relayData = new(formData);

                await Task.Run(() =>
                    _logger.LogInformation("API: {Api} - IP: {IpAddress} - Integration: {IntegrationType}",
                        relayData.Api,
                        HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown",
                        relayData.AppConfig.IntegrationType),
                    cancellationToken);

                MyInvoiceTransformer invoiceTransformer = new();
                MyInvoice invObject = await Task.Run(() =>
                    invoiceTransformer.Transform(relayData),
                    cancellationToken);

                // Get certificate from ApplicationConfig instead of hardcoded file
                string certificateContent = string.Empty;
                string privateKeyContent = string.Empty;

                if (!string.IsNullOrEmpty(AppConfig.Certificate))
                {
                    try
                    {
                        certificateContent = AppConfig.Certificate;
                        privateKeyContent = AppConfig.PrivateKey ?? throw new Exception("Private key is missing from configuration");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to load certificate from ApplicationConfig");
                        throw;
                    }
                }

                if (string.IsNullOrEmpty(certificateContent) || string.IsNullOrEmpty(privateKeyContent))
                {
                    throw new Exception("Certificate or private key is missing from configuration");
                }

                EInvoiceGenerator eInvoice = new(certificateContent, privateKeyContent);
                SubmitDocumentRequest submitDocumentRequest;

                if(AppConfig.DocumentFormat == "JSON"){
                    submitDocumentRequest = eInvoice.GenerateEInvoiceJSON(invObject, (AppConfig.DocumentFormat == "1.1"));
                }else{
                    submitDocumentRequest = eInvoice.GenerateEInvoiceXML(invObject, (AppConfig.DocumentFormat == "1.1"));
                }

                relayData.SubmitRequestJson = JsonConvert.SerializeObject(submitDocumentRequest);

                RelayDataViewModel viewModel = relayData.GetRelayDataViewModel();

                return View("Index", viewModel);

            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Request was cancelled");
                return View("Error", new ErrorViewModel
                {
                    Referrer = formData.GetValueOrDefault("referrer", ""),
                    ErrorMessage = "The request was cancelled. Please try again.",
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }
            catch (JsonSerializationException ex)
            {
                _logger.LogError(ex, "Error processing form data");
                return View("Error", new ErrorViewModel
                {
                    Referrer = formData.GetValueOrDefault("referrer", ""),
                    ErrorMessage = "There was an error processing the invoice data. Please check if all information is filled correctly.",
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("Validation errors: {@Errors}", ex.ValidationErrors);
                return View("Error", new ErrorViewModel
                {
                    Referrer = formData.GetValueOrDefault("Referrer", ""),
                    ValidationErrors = ex.ValidationErrors,
                    ErrorMessage = "Please check the following fields:"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing relay data");
                List<string> validationErrors = new List<string>();

                // Check if it's ArgumentException with validation message
                if (ex is ArgumentException && ex.Message.Contains("Required fields are missing"))
                {
                    validationErrors = ex.Message
                        .Replace("Required fields are missing: ", "")
                        .Split(", ")
                        .ToList();
                }

                return View("Error", new ErrorViewModel
                {
                    Referrer = formData.GetValueOrDefault("Referrer", ""),
                    ValidationErrors = validationErrors,
                    ErrorMessage = "Please check the following fields:",
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }
        }

        [HttpPost("AjaxSubmitInvoice")]
        public async Task<ApiResponse> AjaxSubmitInvoice([FromForm] RelayDataViewModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.InvoiceJson))
                {
                    return BadRequestResponse("Invoice data is null");
                }

                _clientId = model.ClientID;
                _clientSecret = model.ClientSecret;
                _baseUrl = model.IntegrationType.GetApiBaseUrl();

                await GetAccessTokenAsync();

                if (_currentToken?.IsValid != true)
                {
                    return BadRequestResponse("Failed to obtain valid authentication token");
                }

                _logger.LogInformation("Request Payload: {0}", model.SubmitRequestJson);

                // Prepare and submit document
                var submitRequest = JsonConvert.DeserializeObject<SubmitDocumentRequest>(model.SubmitRequestJson);
                HttpRequestMessage request = new(HttpMethod.Post, $"{_baseUrl}/api/v1.0/documentsubmissions");
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                StringContent content = new(JsonConvert.SerializeObject(submitRequest), System.Text.Encoding.UTF8, "application/json");
                request.Content = content;

                HttpResponseMessage response = await _httpClient.SendAsync(request);
                string responseContent = await response.Content.ReadAsStringAsync();

                _logger.LogInformation(
                    "Document submission response - Status: {StatusCode}, Content: {Content}",
                    response.StatusCode,
                    responseContent
                );

                var apiResponse = ProcessJsonResponse(response, responseContent, "Document submitted successfully");

                if (apiResponse.statusCode != HttpStatusCode.Accepted)
                {
                    return apiResponse;
                }

                var typedResponse = JsonConvert.DeserializeObject<SubmitDocumentResponse>(apiResponse.serverResponse);

                if (typedResponse == null || string.IsNullOrEmpty(typedResponse.SubmissionUID))
                {
                    return SubmissionErrorResponse(apiResponse.statusCode, apiResponse.serverResponse);
                }

                var updatedSummary = CreateInvoiceSummary(model, typedResponse);
                model.InvoiceJson = UpdateInvoiceWithResponse(model, updatedSummary);

                return CreateCombinedApiObject(apiResponse, updatedSummary, model);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error submitting invoice");
                return SystemErrorResponse(ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error submitting invoice");
                return SystemErrorResponse(ex);
            }
        }

        [HttpPost("AjaxUpdateStatus")]
        public async Task<ApiResponse> AjaxUpdateStatus([FromForm] RelayDataViewModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.InvoiceSummaryJson))
                {
                    return BadRequestResponse("Invoice summary data is null");
                }

                InvoiceSummary? invoiceSummary = JsonConvert.DeserializeObject<InvoiceSummary>(model.InvoiceSummaryJson);

                if (invoiceSummary is null || string.IsNullOrEmpty(invoiceSummary.SubmissionUid))
                {
                    return BadRequestResponse("Missing submission identifiers");
                }

                _clientId = model.ClientID;
                _clientSecret = model.ClientSecret;
                _baseUrl = model.IntegrationType.GetApiBaseUrl();

                await GetAccessTokenAsync();

                if (_currentToken?.IsValid != true)
                {
                    return BadRequestResponse("Failed to obtain valid authentication token");
                }

                // Get document status
                HttpRequestMessage request = new(HttpMethod.Get, $"{_baseUrl}/api/v1.0/documentsubmissions/{invoiceSummary.SubmissionUid}?pageNo=1&pageSize=1");
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await _httpClient.SendAsync(request);
                string responseContent = await response.Content.ReadAsStringAsync();

                _logger.LogDebug("Document status response - Status: {StatusCode}, Content: {Content}",
                    response.StatusCode,
                    responseContent);

                var apiResponse = ProcessJsonResponse(response, responseContent, "Status retrieved successfully");

                if (apiResponse.statusCode != HttpStatusCode.OK)
                {
                    return apiResponse;
                }

                var typedResponse = JsonConvert.DeserializeObject<DocumentStatusResponse>(apiResponse.serverResponse);

                if (typedResponse == null)
                {
                    return DeserializationErrorResponse(HttpStatusCode.OK, apiResponse.serverResponse);
                }

                UpdateInvoiceSummary(model, invoiceSummary, typedResponse);
                return CreateCombinedApiObject(apiResponse, invoiceSummary, model);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error getting document status");
                return SystemErrorResponse(ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error getting document status");
                return SystemErrorResponse(ex);
            }
        }



        private async Task<TokenInfo> GetAccessTokenAsync()
        {
            try
            {
                // Try to load token from storage first
                var storedToken = TokenInfo.LoadToken(_clientId);
                if (storedToken?.IsValid == true)
                {
                    _logger.LogDebug("Using stored token - Expires at: {ExpiryTime}", storedToken.ExpiresAt);
                    _currentToken = storedToken;
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", _currentToken.AccessToken);
                    return _currentToken;
                }

                // Check if current token is still valid
                if (_currentToken?.IsValid == true)
                {
                    _logger.LogDebug("Using current token - Expires at: {ExpiryTime}", _currentToken.ExpiresAt);
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
                _logger.LogDebug("Requesting new token from: {Url}", request.RequestUri);

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
                _logger.LogInformation("New token generated - Valid until: {ExpiryTime}", _currentToken.ExpiresAt);

                return _currentToken;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Token generation failed");
                throw;
            }
        }

        private static string UpdateInvoiceWithResponse(RelayDataViewModel model, InvoiceSummary updateSummary)
        {
            string invoiceJson = model.InvoiceJson;

            invoiceJson = RelayDataHelper.UpdateOrCreateField(invoiceJson, model.InvoiceTypeCode == InvoiceTypeCodeEnum.Invoice ? "IssueDate" : "Date", updateSummary.DocumentIssueDate[..10]);

            invoiceJson = RelayDataHelper.ModifyStringCustomFields2(invoiceJson, ManagerCustomField.IntegrationTypeGuid, updateSummary.IntegrationType.ToString());
            invoiceJson = RelayDataHelper.ModifyStringCustomFields2(invoiceJson, ManagerCustomField.DocumentFormatGuid, updateSummary.DocumentFormat);
            invoiceJson = RelayDataHelper.ModifyStringCustomFields2(invoiceJson, ManagerCustomField.DocumentVersionGuid, updateSummary.DocumentVersion);

            invoiceJson = RelayDataHelper.ModifyStringCustomFields2(invoiceJson, ManagerCustomField.SubmissionIdGuid, updateSummary.SubmissionUid);
            invoiceJson = RelayDataHelper.ModifyStringCustomFields2(invoiceJson, ManagerCustomField.SubmissionDateGuid, updateSummary.SubmissionDate.ToString("yyyy-MM-dd HH:mm:ssZ"));

            invoiceJson = RelayDataHelper.ModifyStringCustomFields2(invoiceJson, ManagerCustomField.DocumentUUIDGuid, updateSummary.DocumentUUID);
            invoiceJson = RelayDataHelper.ModifyStringCustomFields2(invoiceJson, ManagerCustomField.DocumentIssuedDateGuid, updateSummary.DocumentIssueDate);
            invoiceJson = RelayDataHelper.ModifyStringCustomFields2(invoiceJson, ManagerCustomField.DoucmentStatusGuid, updateSummary.DocumentStatus);
            invoiceJson = RelayDataHelper.ModifyStringCustomFields2(invoiceJson, ManagerCustomField.DocumentLongIdGuid, updateSummary.DocumentLongId);
          
            invoiceJson = RelayDataHelper.ModifyStringCustomFields2(invoiceJson, ManagerCustomField.PublicUrlGuid, updateSummary.PublicUrl);

            return invoiceJson;
        }

        private InvoiceSummary CreateInvoiceSummary(RelayDataViewModel model, SubmitDocumentResponse submitResponse)
        {
            return new InvoiceSummary
            {
                IntegrationType = model.IntegrationType,
                DocumentFormat = model.DocumentFormat,
                DocumentVersion = model.DocumentVersion,
                DocumentIssueDate = model.IssueDate,
                SubmissionUid = submitResponse.SubmissionUID,
                SubmissionDate = DateTime.UtcNow,
                DocumentUUID = submitResponse.AcceptedDocuments[0].UUID,
                DocumentLongId = submitResponse.AcceptedDocuments[0].InvoiceCodeNumber,
                DocumentStatus = "Submitted",
                PublicUrl = string.Empty
            };
        }

        private void UpdateInvoiceSummary(RelayDataViewModel model, InvoiceSummary invoiceSummary, DocumentStatusResponse documentStatusResponse)
        {
            invoiceSummary.DocumentIssueDate = model.IssueDate;
            invoiceSummary.IntegrationType = model.IntegrationType;
            invoiceSummary.DocumentFormat = model.DocumentFormat;
            invoiceSummary.DocumentVersion = model.DocumentVersion;

            invoiceSummary.SubmissionDate = documentStatusResponse.DateTimeReceived;
            invoiceSummary.DocumentUUID = documentStatusResponse.DocumentSummary[0].UUID;
            invoiceSummary.DocumentLongId = documentStatusResponse.DocumentSummary[0].LongId;
            invoiceSummary.DocumentStatus = documentStatusResponse.DocumentSummary[0].Status;

            if (!string.IsNullOrEmpty(invoiceSummary.DocumentLongId))
            {
                string baseUrl = model.IntegrationType.GetQrBaseUrl();
                invoiceSummary.PublicUrl = !string.IsNullOrEmpty(baseUrl)
                    ? $"{baseUrl.TrimEnd('/')}/{invoiceSummary.DocumentUUID}/share/{invoiceSummary.DocumentLongId}"
                    : string.Empty;
            }

            model.InvoiceJson = UpdateInvoiceWithResponse(model, invoiceSummary);
        }

        private ApiResponse CreateCombinedApiObject(ApiResponse submitResponse, InvoiceSummary updatedSummary, RelayDataViewModel model)
        {
            return new ApiResponse
            {
                statusCode = submitResponse.statusCode,
                code = submitResponse.code,
                message = submitResponse.message,
                serverResponse = submitResponse.serverResponse,
                apiInvoice = new ApiResponse.ApiInvoice
                {
                    apiUrl = Utils.ConstructInvoiceApiUrl(model.Referrer, model.FormKey),
                    secretKey = model.Token,
                    payload = model.InvoiceJson
                },
                publicUrl = updatedSummary.PublicUrl,
                invoiceSummary = updatedSummary
            };
        }

        private ApiResponse ProcessJsonResponse(HttpResponseMessage response, string responseContent, string successMessage)
        {
            try
            {
                var jsonResponse = JObject.Parse(responseContent);

                if (response.StatusCode == HttpStatusCode.Accepted)
                {
                    return new ApiResponse
                    {
                        statusCode = response.StatusCode,
                        code = "Accepted",
                        message = successMessage,
                        serverResponse = responseContent
                    };
                }

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return new ApiResponse
                    {
                        statusCode = response.StatusCode,
                        code = "Success",
                        message = successMessage,
                        serverResponse = responseContent
                    };
                }

                return new ApiResponse
                {
                    statusCode = response.StatusCode,
                    code = "ApiError",
                    message = ExtractErrorMessage(jsonResponse),
                    serverResponse = responseContent
                };
            }
            catch (JsonReaderException ex)
            {
                _logger.LogError(ex, "Failed to parse JSON response");
                return HandleErrorResponse(response, responseContent);
            }
        }


        private static string ExtractErrorMessage(JObject response)
        {
            return response.SelectToken("$.error.details[0].message")?.ToString()
                ?? response.SelectToken("$.error.message")?.ToString()
                ?? response.SelectToken("$.error.errordetails[0].error")?.ToString()
                ?? response.SelectToken("$.error.errordetails[0].errorMs")?.ToString()
                ?? response.SelectToken("$.message")?.ToString()
                ?? response.SelectToken("$.error")?.ToString()
                ?? "API request failed";
        }
        
        private ApiResponse HandleErrorResponse(HttpResponseMessage response, string responseContent)
        {
            _logger.LogDebug("Raw error response: {Content}", responseContent);

            try
            {
                var jObject = JObject.Parse(responseContent);

                return new ApiResponse
                {
                    statusCode = response.StatusCode,
                    code = "ApiError",
                    message = ExtractErrorMessage(jObject),
                    serverResponse = jObject.ToString()
                };
            }
            catch (JsonReaderException ex)
            {
                _logger.LogError(ex, "Failed to parse error response");
                string jsonString = $"{{\"error\": \"Failed to parse error response\", \"message\": \"{ex.Message}\"}}";
                return new ApiResponse
                {
                    statusCode = response.StatusCode,
                    code = "ApiError",
                    message = "Failed to parse error response",
                    serverResponse = jsonString
                };
            }
        }

        private ApiResponse BadRequestResponse(string message)
        {
            string jsonString = $"{{\"error\": \"{message}\"}}";

            return new ApiResponse
            {
                statusCode = HttpStatusCode.BadRequest,
                code = "ValidationError",
                message = message,
                serverResponse = jsonString
            };
        }

        private ApiResponse DeserializationErrorResponse(HttpStatusCode statusCode, dynamic submitResponse)
        {
            string jsonString = $"{{\"error\": \"DeserializationError\", \"message\": \"Unable to process the server response\"}}";

            return new ApiResponse
            {
                statusCode = statusCode,
                code = "DeserializationError",
                message = "Unable to process the server response. Please try again.",
                serverResponse = jsonString
            };
        }

        private ApiResponse SubmissionErrorResponse(HttpStatusCode statusCode, dynamic submitResponse)
        {
            string jsonString = $"{{\"error\": \"SubmissionError\", \"message\": \"Failed to get submission ID\"}}";

            return new ApiResponse
            {
                statusCode = statusCode,
                code = "SubmissionError",
                message = "Failed to get submission ID",
                serverResponse = jsonString
            };
        }

        private ApiResponse SystemErrorResponse(Exception ex)
        {
            string jsonString = $"{{\"error\": \"SystemError\", \"message\": \"{ex.Message}\"}}";

            return new ApiResponse
            {
                statusCode = HttpStatusCode.InternalServerError,
                code = "SystemError",
                message = "An unexpected error occurred. Please try again later.",
                serverResponse = jsonString
            };
        }
    }



public class ApiResponse
{
    public HttpStatusCode statusCode { get; set; }
    public string code { get; set; }
    public string message { get; set; }
    public ApiInvoice? apiInvoice { get; set; }
    public string? publicUrl { get; set; }
    public InvoiceSummary? invoiceSummary { get; set; }
    public string? serverResponse { get; set; }

    public class ApiInvoice
    {
        public string apiUrl { get; set; }
        public string secretKey { get; set; }
        public string payload { get; set; }
    }
}

}