using InvoisEGS.ApiClient.ApiHelpers;
using InvoisEGS.ApiClient.ApiModels;
using InvoisEGS.Models;
using InvoisEGS.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace InvoisEGS.Controllers
{
    public class SetupController : Controller
    {
        private readonly ILogger<SetupController> _logger;

        public SetupController(ILogger<SetupController> logger)
        {
            _logger = logger;
        }

        [HttpGet("Register")]
        public IActionResult Register()
        {
            string? setupViewModelJson = TempData["SetupViewModel"] as string;

            if (!string.IsNullOrEmpty(setupViewModelJson))
            {
                SetupViewModel? setupViewModel = JsonConvert.DeserializeObject<SetupViewModel>(setupViewModelJson);
                return View("Register", setupViewModel);
            }
            return View();
        }

        [HttpPost("Update")]
        public IActionResult UpdateCustomField([FromForm] Dictionary<string, string> formData)
        {
            try
            {
                string Referrer = Utils.GetValue(formData, "Referrer");
                string FormKey = Utils.GetValue(formData, "Key");
                string Data = Utils.GetValue(formData, "Data");
                string Api = Utils.GetValue(formData, "Api");
                string Token = Utils.GetValue(formData, "Token");

                _logger.LogInformation("Setup API Diakses oleh : {api} IP address: {ipAddress}", Api, HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown");

                string BusinessDetailJson = RelayDataHelper.GetValueJson(Data, "BusinessDetails");

                if (!string.IsNullOrEmpty(FormKey))
                {
                    SetupViewModel setupViewModel = new()
                    {
                        Referrer = Referrer,
                        Api = Api,
                        Token = Token,
                        BusinessDetailJson = BusinessDetailJson
                    };

                    return View("UpdateBusinessData", setupViewModel);
                }

                return View("index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kesalahan saat memproses setup api");
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult UpdateBusinessDetail([FromForm] SetupViewModel viewModel)
        {
            try
            {
                if (viewModel == null)
                {
                    return Json(new { success = false, message = "Invalid data" });
                }

                string businessDetails = viewModel.BusinessDetailJson;

                if (string.IsNullOrEmpty(businessDetails))
                {
                    businessDetails = @"{
                        ""Name"": """",
                        ""Address"": """",
                        ""CustomFields2"": {
                            ""Decimals"": { },
                            ""Strings"": {
                                ""7351fc99-f61d-44b8-96ad-0e10c7c3fc9a"": """",
                                ""12a2b5fb-e388-4e72-94ff-8337462fb543"": """"
                            }
                        }
                    }";
                }

                // Update business details with form data
                businessDetails = RelayDataHelper.UpdateOrCreateField(businessDetails, "Name", viewModel.RegistrationName ?? "");
                //businessDetails = RelayDataHelper.UpdateOrCreateField(businessDetails, "Address", viewModel.GetFormattedAddress());
                ApplicationConfig applicationConfig = viewModel.GetApplicationConfig();
                
                string? serializedConfig = applicationConfig != null ? ObjectCompressor.SerializeToBase64String(applicationConfig) : string.Empty;
                businessDetails = RelayDataHelper.ModifyStringCustomFields2(businessDetails, ManagerCustomField.AppConfigGuid, serializedConfig ?? string.Empty);
                businessDetails = RelayDataHelper.ModifyStringCustomFields2(businessDetails, ManagerCustomField.AppVersionGuid, VersionHelper.GetVersion());

                var combinedApiObject = new
                {
                    ApiBusinessDetails = new
                    {
                        ApiUrl = $"{viewModel.Api}/business-details-form/38cf4712-6e95-4ce1-b53a-bff03edad273",
                        SecretKey = viewModel.Token,
                        Payload = businessDetails
                    }
                };

                return Ok(JsonConvert.SerializeObject(combinedApiObject, Formatting.Indented));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating business detail");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("setup/getcfdata")]
        public IActionResult CustomFieldJson()
        {
            try
            {
                string jsonData = XmlJsonUtilities.LoadEmbededResources("InvoisEGS.ApiClient.Resources.CfData.json");
                return Content(jsonData, "application/json");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to load resource: {ex.Message}");
            }
        }


        [HttpPost]
        public async Task<IActionResult> GetAccessToken([FromBody] TokenRequest request)
        {
            try
            {
                if (request == null || string.IsNullOrEmpty(request.ClientId) || string.IsNullOrEmpty(request.ClientSecret))
                {
                    return Json(new { success = false, message = "Invalid credentials" });
                }

                using HttpClient client = new();
                FormUrlEncodedContent content = new(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials"),
                    new KeyValuePair<string, string>("client_id", request.ClientId),
                    new KeyValuePair<string, string>("client_secret", request.ClientSecret),
                    new KeyValuePair<string, string>("scope", "InvoicingAPI")
                });

                // Parse string to enum
                IntegrationType integrationType = Enum.Parse<IntegrationType>(request.IntegrationType);
                string baseUrl = integrationType.GetApiBaseUrl();

                HttpResponseMessage response = await client.PostAsync($"{baseUrl}/connect/token", content);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return Json(new { success = true, data = result });
                }

                string errorContent = await response.Content.ReadAsStringAsync();
                return Json(new { success = false, message = $"Failed to get access token. {errorContent}" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> VerifyCertificate(IFormFile certificateFile, string? password, string taxpayerTIN, string certificateType, IFormFile? privateKeyFile = null)
        {
            try
            {
                if (certificateFile == null || certificateFile.Length == 0)
                    return Json(new { success = false, message = "No certificate file provided" });

                using var ms = new MemoryStream();
                await certificateFile.CopyToAsync(ms);
                var certBytes = ms.ToArray();

                try
                {
                    if (certificateType.ToLower().Equals("pfx") || certificateType.ToLower().Equals("p12"))
                    {
                        using var cert = new X509Certificate2(certBytes, password, X509KeyStorageFlags.Exportable);
                        var certInfo = CertificateHandler.ValidateCertificate(cert, taxpayerTIN);

                        if (!certInfo.IsValid)
                        {
                            return Json(new { success = false, message = "Certificate validation failed" });
                        }

                        var (certificateBase64, privateKeyBase64) = CertificateHandler.GetCertificateContents(certBytes, password ?? "");
                        
                        // Clean up private key
                        string cleanedPrivateKey = Convert.ToBase64String(Encoding.UTF8.GetBytes(privateKeyBase64));

                        return Json(new
                        {
                            success = true,
                            data = new
                            {
                                subject = certInfo.Subject,
                                issuer = certInfo.Issuer,
                                validFrom = certInfo.ValidFrom,
                                validTo = certInfo.ValidTo,
                                serialNumber = certInfo.SerialNumber,
                                isValid = certInfo.IsValid,
                                tinMatch = certInfo.TaxpayerTIN == taxpayerTIN,
                                taxpayerTIN = certInfo.TaxpayerTIN,
                                certificateBase64,
                                privateKeyBase64 = cleanedPrivateKey
                            }
                        });
                    }
                    else // PEM certificate
                    {
                        if (privateKeyFile == null)
                            return Json(new { success = false, message = "Private key file is required for PEM certificates" });

                        using var privateKeyStream = new MemoryStream();
                        await privateKeyFile.CopyToAsync(privateKeyStream);
                        var privateKeyBytes = privateKeyStream.ToArray();

                        using var cert = new X509Certificate2(certBytes);
                        var certInfo = CertificateHandler.ValidateCertificate(cert, taxpayerTIN);

                        if (!certInfo.IsValid)
                        {
                            return Json(new { success = false, message = "Certificate validation failed" });
                        }

                        //string privateKeyString = System.Text.Encoding.UTF8.GetString(privateKeyBytes);
                        string cleanedPrivateKey = Convert.ToBase64String(privateKeyBytes);

                        return Json(new
                        {
                            success = true,
                            data = new
                            {
                                subject = certInfo.Subject,
                                issuer = certInfo.Issuer,
                                validFrom = certInfo.ValidFrom,
                                validTo = certInfo.ValidTo,
                                serialNumber = certInfo.SerialNumber,
                                isValid = certInfo.IsValid,
                                tinMatch = certInfo.TaxpayerTIN == taxpayerTIN,
                                taxpayerTIN = certInfo.TaxpayerTIN,
                                certificateBase64 = Convert.ToBase64String(certBytes),
                                privateKeyBase64 = cleanedPrivateKey
                            }
                        });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Certificate verification failed for TIN: {TIN}", taxpayerTIN);
                    return Json(new { success = false, message = $"Certificate verification failed: {ex.Message}" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during certificate verification");
                return Json(new { success = false, message = "An unexpected error occurred during certificate verification" });
            }
        }


    }
}

