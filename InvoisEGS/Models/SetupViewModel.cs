using InvoisEGS.ApiClient.ApiModels;
using InvoisEGS.ApiClient.XModels;

namespace InvoisEGS.Models
{
    public class SetupViewModel
    {
        public string Referrer { get; set; }
        public string Api { get; set; }
        public string Token { get; set; }
        public string BusinessDetailJson { get; set; }
        public long EGSVersion { get; set; }
        public string SignServiceUrl { get; set; }
        public string Certificate { get; set; }
        public string PrivateKey { get; set; }

        // Credential Verification
        public IntegrationType IntegrationType { get; set; } = IntegrationType.PreProduction;
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string DocumentFormat { get; set; } = "XML";
         public string DocumentVersion { get; set; } = "1.0";
        // Accounting Supplier Party
        public string TaxPayerTIN { get; set; } 
        public string RegistrationName { get; set; } = "WXXX_XXXXNI";
        public string TaxPayerIDType { get; set; } = "BRN";
        public string TaxPayerIDNumber { get; set; }

        public string TaxPayerSST { get; set; } = "NA";
        public string TaxPayerTTX { get; set; } = "NA";

        public string IndustryClassificationCode { get; set; } = "00000";
        public string IndustryClassificationName { get; set; } = "Not Applicable";

        public string AddAccountAgencyName { get; set; } = "CertEX";
        public string AddAccountID { get; set; } = "NA";

        public string CountryIdCode { get; set; } = "MYS";
        public string CountryIdListID { get; set; } = "ISO3166-1";
        public string CountryIdListAgencyID { get; set; } = "6";
        public string PostalZone { get; set; } = "50480";
        public string CountrySubentityCode { get; set; } = "01";
        public string CityName { get; set; } = "GELANG PATAH";

        public string AddressLine1 { get; set; } = "NO.36 JALAN LAMAN SETIA 7/6";
        public string AddressLine2 { get; set; } = "TAMAN LAMAN SETIA";
        public string AddressLine3 { get; set; } = "";
        public string ContactTelephone { get; set; } = "+60123456789";
        public string ContactElectronicMail { get; set; } = "wma@gowmaz.com";

        public ApplicationConfig GetApplicationConfig()
        {
            return new ApplicationConfig
            {
                IntegrationType = IntegrationType,
                ClientID = ClientId,
                ClientSecret = ClientSecret,
                SignServiceUrl = SignServiceUrl,
                DocumentFormat = DocumentFormat,
                DocumentVersion = DocumentVersion,    
                Certificate = Certificate,
                PrivateKey = PrivateKey,
                Supplier = new AccountingSupplierParty
                {
                    AdditionalAccountID = new List<AdditionalAccountID> { new AdditionalAccountID(AddAccountID, AddAccountAgencyName) },
                    Party = new List<SupplierParty> 
                    {
                        new SupplierParty
                        {
                            IndustryClassificationCode = new List<IndustryClassificationCode> { new IndustryClassificationCode(IndustryClassificationCode, IndustryClassificationName) },
                            PartyIdentification = new List<PartyIdentification>
                            {
                                new PartyIdentification(TaxPayerTIN, "TIN"),
                                new PartyIdentification(TaxPayerIDNumber, TaxPayerIDType),
                                new PartyIdentification(TaxPayerSST, "SST"),
                                new PartyIdentification(TaxPayerTTX, "TTX")
                            },
                            PostalAddress = new List<PostalAddress>
                            {
                                new PostalAddress
                                {
                                    CityName = new List<TextValue> { new TextValue(CityName) },
                                    PostalZone = new List<TextValue> { new TextValue(PostalZone) },
                                    CountrySubentityCode = new List<TextValue> { new TextValue(CountrySubentityCode) },
                                    AddressLine = new List<AddressLine>
                                    {
                                        new AddressLine(AddressLine1),
                                        new AddressLine(AddressLine2),
                                        new AddressLine(AddressLine3)
                                    },
                                    Country = new List<Country> { new Country(CountryIdCode) }
                                }
                            },
                            PartyLegalEntity = new List<PartyLegalEntity>
                            {
                                new PartyLegalEntity(RegistrationName)
                            },
                            Contact = new List<Contact>
                            {
                                new Contact
                                {
                                    Telephone = new List<TextValue> { new TextValue(ContactTelephone) },
                                    ElectronicMail = new List<TextValue> { new TextValue(ContactElectronicMail) }
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}
