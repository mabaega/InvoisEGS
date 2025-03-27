using InvoisEGS.ApiClient.ApiModels;

namespace InvoisEGS.ApiClient.ApiHelpers
{
    public static class IntegrationTypeExtensions
    {
        private const string API_DOMAIN = "api.myinvois.hasil.gov.my";
        private const string QR_DOMAIN = ".myinvois.hasil.gov.my";

        public static string GetEnvironmentPrefix(this IntegrationType type)
        {
            return type == IntegrationType.PreProduction ? "preprod" : "";
        }

        public static string GetApiBaseUrl(this IntegrationType type)
        {
            return $"https://{type.GetEnvironmentPrefix()}-{API_DOMAIN}";
        }

        public static string GetQrBaseUrl(this IntegrationType type)
        {
            return $"https://{type.GetEnvironmentPrefix()}{QR_DOMAIN}";
        }
    }
}