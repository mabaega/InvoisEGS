using System.Security.Cryptography.X509Certificates;

namespace InvoisEGS.ApiClient.ApiModels
{
    public class CertificateInfo
    {
        public string Subject { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string SerialNumber { get; set; } = string.Empty;
        public bool IsValid { get; set; }
        public string TaxpayerTIN { get; set; } = string.Empty;
        public X509Certificate2 Certificate { get; set; } = null!;
    }
}