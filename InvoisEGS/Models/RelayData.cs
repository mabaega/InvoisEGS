using InvoisEGS.ApiClient.ApiModels;
using InvoisEGS.ApiClient.XModels;
using InvoisEGS.Exceptions;
using InvoisEGS.Utilities;
using Newtonsoft.Json;
using System.Collections.Immutable;

namespace InvoisEGS.Models
{
    public class RelayData
    {
        public string Referrer { get; private set; } = string.Empty;
        public string FormKey { get; private set; } = string.Empty;
        public string Api { get; private set; } = string.Empty;
        public string Token { get; private set; } = string.Empty;
        public string InvoiceJson { get; set; } = string.Empty;
        public string BusinessDetailJson { get; set; } = string.Empty;
        public string SubmitRequestJson { get; set; } = string.Empty;
        public ApplicationConfig AppConfig { get; set; }
        public long EGSVersion { get; private set; }
        public ManagerInvoice ManagerInvoice { get; private set; }

        public string LocalIssueDate { get; private set; }
        public string DocumentCurrencyCode { get; private set; } = "MYR";
        public string TaxCurrencyCode { get; private set; } = "MYR";
        public decimal ExchangeRate { get; private set; } = 1;

        public InvoiceTypeCodeEnum InvoiceTypeCode { get; private set; } = InvoiceTypeCodeEnum.Invoice;
        public string ListVersionID { get; private set; } = "1.1";
        public decimal InvoiceTotal { get; private set; } = 0;

        public AccountingCustomerParty Buyer { get; private set; }
        public InvoiceSummary InvoiceSummary { get; set; } = new InvoiceSummary();
        public List<string> CNDNReferences { get; internal set; }

        private readonly List<string> validationErrors = [];
        public RelayData() { }
        public RelayData(Dictionary<string, string> formData)
        {
            try
            {
                Referrer = Utils.GetValue(formData, "Referrer");
                FormKey = Utils.GetValue(formData, "Key");
                Api = Utils.GetValue(formData, "Api");
                Token = Utils.GetValue(formData, "Token");

                string invoiceView = Utils.GetValue(formData, "View");
                InvoiceTotal = RelayDataHelper.ParseTotalValue(invoiceView);

                string Data = Utils.GetValue(formData, "Data");

                BusinessDetailJson = RelayDataHelper.GetValueJson(Data, "BusinessDetails");
                (AppConfig, EGSVersion) = RelayDataHelper.InitializeBusinessData(BusinessDetailJson);

                InvoiceJson = RelayDataHelper.FindStringValueInJson(Data, FormKey) ?? "";

                string mergedJson = RelayDataHelper.GetJsonDataByGuid(Data, FormKey);

                ManagerInvoice = JsonConvert.DeserializeObject<ManagerInvoice>(mergedJson);
                ExchangeRate = ManagerInvoice.ExchangeRate;


                if (string.IsNullOrEmpty(ManagerInvoice?.Reference))
                {
                    validationErrors.Add("Invoice Reference is required");
                }

                if (ManagerInvoice?.Lines?.Count == 0)
                {
                    validationErrors.Add("Invoice must have at least one item");
                }

                if (InvoiceTotal == 0)
                {
                    validationErrors.Add("Invoice total amount cannot be zero");
                }

                // Basic validations first
                ValidateBasicRequirements();
                
                ValidateItems();

                if (validationErrors.Count > 0)
                {
                    throw new ValidationException("Required fields are missing", validationErrors);
                }

                InitializeInvoiceData(mergedJson);

                DetermineInvoiceType(mergedJson);

                ValidatePartyIDsSync().GetAwaiter().GetResult();

                if (validationErrors.Count > 0)
                {
                    throw new ValidationException("Validation failed", validationErrors);
                }

            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to initialize RelayData", ex);
            }
        }
        
        private void ValidateBasicRequirements()
        {
            if (string.IsNullOrEmpty(ManagerInvoice?.Reference))
            {
                validationErrors.Add("Invoice Reference is required");
            }

            if (ManagerInvoice?.Lines?.Count == 0)
            {
                validationErrors.Add("Invoice must have at least one item");
            }

            if (InvoiceTotal == 0)
            {
                validationErrors.Add("Invoice total amount cannot be zero");
            }
        }

        private void InitializeInvoiceData(string jsonInvoice)
        {
            if (string.IsNullOrEmpty(jsonInvoice))
            {
                throw new ArgumentException("Invoice data is required");
            }

            try
            {
                DocumentCurrencyCode = RelayDataHelper.FindStringValueInJson(jsonInvoice, "Code", "Currency") ?? "MYR";

                var integrationTypeString = RelayDataHelper.GetStringCustomField2Value(jsonInvoice, ManagerCustomField.IntegrationTypeGuid) ?? "PreProduction";
                InvoiceSummary.IntegrationType = Enum.TryParse(integrationTypeString, out IntegrationType integrationType) ? integrationType : AppConfig.IntegrationType;
                
                InvoiceSummary.DocumentFormat  = RelayDataHelper.GetStringCustomField2Value(jsonInvoice, ManagerCustomField.DocumentFormatGuid) ?? AppConfig.DocumentFormat ?? "JSON";
                InvoiceSummary.DocumentVersion = RelayDataHelper.GetStringCustomField2Value(jsonInvoice, ManagerCustomField.DocumentVersionGuid) ?? AppConfig.DocumentVersion ?? "1.0";

                LocalIssueDate = RelayDataHelper.GetStringCustomField2Value(jsonInvoice, ManagerCustomField.DocumentIssuedDateGuid);

                if (string.IsNullOrEmpty(LocalIssueDate))
                {
                    var idt = RelayDataHelper.FindStringValueInJson(jsonInvoice, "Date") ?? RelayDataHelper.FindStringValueInJson(jsonInvoice, "IssueDate");
                    if (DateTime.TryParse(idt, out DateTime parsetdt))
                    {
                        LocalIssueDate = parsetdt.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    else
                    {
                        LocalIssueDate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }

                InvoiceSummary.DocumentIssueDate = LocalIssueDate;

                InvoiceSummary.SubmissionUid = RelayDataHelper.GetStringCustomField2Value(jsonInvoice, ManagerCustomField.SubmissionIdGuid) ?? "";

                string submissionDate = RelayDataHelper.GetStringCustomField2Value(jsonInvoice, ManagerCustomField.SubmissionDateGuid);

                if (DateTime.TryParseExact(submissionDate, "yyyy-MM-dd HH:mm:ssZ", null, System.Globalization.DateTimeStyles.None, out DateTime parsedSubmissionDate))
                {
                    InvoiceSummary.SubmissionDate = parsedSubmissionDate;
                }

                InvoiceSummary.DocumentUUID = RelayDataHelper.GetStringCustomField2Value(jsonInvoice, ManagerCustomField.DocumentUUIDGuid) ?? "";
                InvoiceSummary.DocumentLongId = RelayDataHelper.GetStringCustomField2Value(jsonInvoice, ManagerCustomField.DocumentLongIdGuid) ?? "";
                InvoiceSummary.DocumentStatus = RelayDataHelper.GetStringCustomField2Value(jsonInvoice, ManagerCustomField.DoucmentStatusGuid) ?? "";
                InvoiceSummary.PublicUrl = RelayDataHelper.GetStringCustomField2Value(jsonInvoice, ManagerCustomField.PublicUrlGuid) ?? "";

                GetBuyer(jsonInvoice);
            }
            catch (ValidationException)
            {
                throw;  // Re-throw ValidationException directly
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to initialize invoice data", ex);
            }
        }
        private void GetBuyer(string jsonInvoice)
        {
            Buyer = new AccountingCustomerParty
            {
                Party = new List<CustomerParty>
                {
                    new CustomerParty
                    {
                        PartyIdentification = new List<PartyIdentification>
                        {
                            new PartyIdentification(RelayDataHelper.GetStringCustomField2Value(jsonInvoice, ManagerCustomField.BuyerTINGuid, "InvoiceParty") ?? "NA", "TIN"),
                            new PartyIdentification(RelayDataHelper.GetStringCustomField2Value(jsonInvoice, ManagerCustomField.BuyerSSTGuid, "InvoiceParty") ?? "NA", "SST"),
                        },
                        PostalAddress = new List<PostalAddress>
                        {
                            new PostalAddress
                            {
                                CityName = new List<TextValue> { new TextValue { Value = RelayDataHelper.GetStringCustomField2Value(jsonInvoice, ManagerCustomField.BuyerCityNameGuid, "InvoiceParty") } },
                                PostalZone = new List<TextValue> { new TextValue { Value = RelayDataHelper.GetStringCustomField2Value(jsonInvoice, ManagerCustomField.BuyerPostalZoneGuid, "InvoiceParty") } },
                                CountrySubentityCode = new List<TextValue> { new TextValue { Value = RelayDataHelper.GetStringCustomField2Value(jsonInvoice, ManagerCustomField.BuyerSubentityCodeGuid, "InvoiceParty") } },
                                AddressLine = new List<AddressLine>
                                {
                                    new AddressLine(RelayDataHelper.GetStringCustomField2Value(jsonInvoice, ManagerCustomField.BuyerAddressLine1Guid, "InvoiceParty") ?? ""),
                                    new AddressLine(RelayDataHelper.GetStringCustomField2Value(jsonInvoice, ManagerCustomField.BuyerAddressLine2Guid, "InvoiceParty") ?? ""),
                                    new AddressLine(RelayDataHelper.GetStringCustomField2Value(jsonInvoice, ManagerCustomField.BuyerAddressLine3Guid, "InvoiceParty") ?? ""),
                                },
                                Country = new List<Country>
                                {
                                    new Country(RelayDataHelper.GetStringCustomField2Value(jsonInvoice, ManagerCustomField.BuyerCountryGuid, "InvoiceParty"))
                                }
                            }
                        },
                        PartyLegalEntity = new List<PartyLegalEntity>
                        {
                            new PartyLegalEntity
                            {
                                RegistrationName = new List<TextValue> { new TextValue { Value = RelayDataHelper.GetStringCustomField2Value(jsonInvoice, ManagerCustomField.BuyerRegistrationNameGuid, "InvoiceParty") } }
                            }
                        },
                        Contact = new List<Contact>
                        {
                            new Contact(
                                RelayDataHelper.GetStringCustomField2Value(jsonInvoice, ManagerCustomField.BuyerTeleponGuid, "InvoiceParty"),
                                RelayDataHelper.GetStringCustomField2Value(jsonInvoice, ManagerCustomField.BuyerEmailGuid, "InvoiceParty")
                            )
                        }
                    }
                }
            };

            var IdType = RelayDataHelper.GetStringCustomField2Value(jsonInvoice, ManagerCustomField.BuyerIDTypeGuid, "InvoiceParty") ?? ""; //NRIC, BRN, PASSPORT, ARMY

            PartyIdentification partyIdentification = new PartyIdentification(RelayDataHelper.GetStringCustomField2Value(jsonInvoice, ManagerCustomField.BuyerIDNumberGuid, "InvoiceParty") ?? "NA", IdType);

            Buyer.Party[0].PartyIdentification.Add(partyIdentification);
        }

        private void DetermineInvoiceType(string jsonInvoice)
        {
            if (Referrer.Contains("credit-note-view"))
            {
                InvoiceTypeCode = InvoiceTypeCodeEnum.CreditNote;

                string salesUnitPrice = RelayDataHelper.FindStringValueInJson(jsonInvoice, "UnitPrice");
                if (decimal.TryParse(salesUnitPrice, out decimal salesUnitPriceDecimal) && salesUnitPriceDecimal < 0)
                {
                    InvoiceTypeCode = InvoiceTypeCodeEnum.DebitNote;
                }

                string RefDocumentUUID = RelayDataHelper.GetStringCustomField2Value(jsonInvoice, ManagerCustomField.DocumentUUIDGuid, "RefInvoice") ?? "";

                if (string.IsNullOrEmpty(RefDocumentUUID))
                {
                    throw new ValidationException("Missing Document References (Document UUID) from Debited/Credited Invoice.", validationErrors);
                }

                CNDNReferences = new List<string> { RefDocumentUUID };

            }
        }

        private async Task ValidatePartyIDsSync()
        {
            // Console.WriteLine("Starting ValidateBuyerIDsSync");
            using var httpClient = new HttpClient();

            var partyTIN = Buyer.Party[0].PartyIdentification
                .FirstOrDefault(x => x.ID[0].SchemeID == "TIN")?.ID[0].Value;

            // Console.WriteLine($"Buyer TIN: {partyTIN}");

            if (string.IsNullOrEmpty(partyTIN) || partyTIN == "NA")
            {
                validationErrors.Add("Buyer TIN is required");
                return;
            }

            var userTIN = AppConfig.Supplier.Party[0].PartyIdentification
                .FirstOrDefault(x => x.ID[0].SchemeID == "TIN")?.ID[0].Value;

            // Console.WriteLine($"User TIN: {userTIN}");

            var partyIDHelper = new PartyIDHelper(httpClient, userTIN, AppConfig.ClientID, AppConfig.ClientSecret,
                AppConfig.IntegrationType);

            // Get ID type and value from Buyer's PartyIdentification
            var partyID = Buyer.Party[0].PartyIdentification
                .FirstOrDefault(x => x.ID[0].SchemeID != "TIN" && x.ID[0].SchemeID != "SST");

            var idType = partyID?.ID[0].SchemeID ?? "";
            var idValue = partyID?.ID[0].Value ?? "";

            // Console.WriteLine($"ID Type: {idType}, ID Value: {idValue}");

            if (!string.IsNullOrEmpty(idType) && !string.IsNullOrEmpty(idValue) && idValue != "NA")
            {
                // Console.WriteLine("Calling GetPartyID");
                var idValidation = await partyIDHelper.GetPartyID(idType, idValue, partyTIN);
                // Console.WriteLine($"GetPartyID result: {idValidation != null}");
                
               if (idValidation == null)
                {
                    validationErrors.Add($"Invalid {idType} ({idValue}) provided for Party TIN: {partyTIN}");
                }
            }
            else
            {
                // Console.WriteLine("Skipping validation: ID Type or Value is empty or NA");
            }
        }

        private void ValidateItems()
        {
            foreach (Line line in ManagerInvoice.Lines)
            {
                string TaxType = line.TaxCode?.CustomFields2?.Strings?.GetValueOrDefault(ManagerCustomField.TaxTypeGuid) ?? string.Empty;
                string ExemptionReason = line.Item?.CustomFields2?.Strings?.GetValueOrDefault(ManagerCustomField.ExemptionReasonGuid) ?? string.Empty;
                string ItemClass = line.Item?.CustomFields2?.Strings?.GetValueOrDefault(ManagerCustomField.CommodityClassGuid) ?? string.Empty;
                string BaseUnit = line.Item?.CustomFields2?.Strings?.GetValueOrDefault(ManagerCustomField.UOMCodeGuid) ?? line.Item?.UnitName ?? string.Empty;

                if (string.IsNullOrEmpty(TaxType))
                {
                    validationErrors.Add($"TaxCategory ID is required  for {line?.Item?.ItemName?? line?.Item?.Name}");
                    continue;
                }

                if (TaxType == "E" && string.IsNullOrEmpty(ExemptionReason))
                {
                    validationErrors.Add("ExemptionReason is required for item with TaxCategory ID E");
                    continue;
                }

                if (string.IsNullOrEmpty(ItemClass))
                {
                    validationErrors.Add($"ItemClass is required for {line?.Item?.ItemName?? line?.Item?.Name}");
                    continue;
                }

                if (!ItemClassReference.ValidateItemClass(ItemClass))
                {
                    validationErrors.Add($"Invalid ItemClass for {line?.Item?.ItemName?? line?.Item?.Name}");
                }

                if (string.IsNullOrEmpty(BaseUnit))
                {
                    validationErrors.Add($"Base Unit is required for {line?.Item?.ItemName?? line?.Item?.Name}");
                    continue;
                }

                if (!BaseUnitReference.ValidateBaseUnit(BaseUnit))
                {
                    validationErrors.Add($"Invalid Base Unit '{BaseUnit}' for {line?.Item?.ItemName?? line?.Item?.Name}");
                }
            }
        }

        public ApprovedInvoiceViewModel GetApprovedInvoiceViewModel()
        {
            ApprovedInvoiceViewModel viewModel = new()
            {
                SubmitDocumentRequestJson = InvoiceJson,
                Referrer = Referrer,
                InvoiceSummary = InvoiceSummary,
            };
            return viewModel;
        }

        public RelayDataViewModel GetRelayDataViewModel()
        {
            RelayDataViewModel viewModel = new();

            //MyInvoice Invoice = JsonConvert.DeserializeObject<MyInvoice>(InvoiceJson);

            viewModel.Referrer = Referrer;
            viewModel.FormKey = FormKey;
            viewModel.Api = Api;
            viewModel.Token = Token;

            viewModel.IntegrationType = AppConfig.IntegrationType;
            viewModel.DocumentFormat = AppConfig.DocumentFormat;
            viewModel.DocumentVersion = AppConfig.DocumentVersion;

            viewModel.ClientID = AppConfig.ClientID;
            viewModel.ClientSecret = AppConfig.ClientSecret;

            //viewModel.AppConfig = AppConfig;

            viewModel.SubmitRequestJson = SubmitRequestJson;
            viewModel.InvoiceJson = InvoiceJson;
            viewModel.BusinessDetailJson = BusinessDetailJson;

            viewModel.InvoiceTypeCode = InvoiceTypeCode;
            viewModel.ListVersionID = ListVersionID;

            viewModel.DocumentReferences = CNDNReferences;

            //Client local time
            viewModel.IssueDate = LocalIssueDate;

            viewModel.DocumentCurrencyCode = DocumentCurrencyCode;
            viewModel.TaxCurrencyCode = TaxCurrencyCode;

            viewModel.InvoiceSummaryJson = JsonConvert.SerializeObject(InvoiceSummary);

            return viewModel;
        }
    }
}