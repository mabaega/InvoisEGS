using InvoisEGS.ApiClient.ApiModels;
using InvoisEGS.ApiClient.XModels;

namespace InvoisEGS.Models
{
    public class ApplicationConfig
    {
        public IntegrationType IntegrationType { get; set; } = IntegrationType.PreProduction;
        public AccountingSupplierParty Supplier { get; set; }
        public string ClientID { get; set; }
        public string ClientSecret { get; set; }
        public string DocumentFormat { get; set; } = "JSON";
        public string DocumentVersion { get; set; } = "1.0";
        public string Certificate { get; set; } //x509Certificate2 base64
        public string PrivateKey { get; set; } //PrivateKey base64
        public string SignServiceUrl { get; set; }
    }
}