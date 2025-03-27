using InvoisEGS.ApiClient.ApiModels;

namespace InvoisEGS.Models
{
    public class RelayDataViewModel
    {
        public string Referrer { get; set; } = string.Empty;
        public string FormKey { get; set; } = string.Empty;
        public string Api { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public IntegrationType IntegrationType { get; set; }
        //public ApplicationConfig AppConfig { get; set; }
        public string ClientID { get; set; }
        public string ClientSecret { get; set; }
        public string SignServiceUrl { get; set; }
        public InvoiceTypeCodeEnum InvoiceTypeCode { get; set; }
        public string ListVersionID { get; set; }
        public List<string> DocumentReferences { get; set; } = new List<string>();
        public string IssueDate { get; set; }
        public string IssueTime { get; set; }
        public string DocumentFormat { get; set; }
        public string DocumentVersion { get; set; }
        public string DocumentCurrencyCode { get; set; }
        public string TaxCurrencyCode { get; internal set; }
        public decimal TotalTaxAmount { get; internal set; }
        public string InvoiceJson { get; set; }
        public string BusinessDetailJson { get; set; }
        public string ServerResponseJson { get; set; } = string.Empty;
        public string SubmitRequestJson { get; set; } = string.Empty;
        public string InvoiceSummaryJson { get; set; }

    }
}