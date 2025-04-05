using InvoisEGS.ApiClient.ApiHelpers;
using InvoisEGS.ApiClient.ApiModels;
using Newtonsoft.Json;

namespace InvoisEGS.Models
{
    public class InvoiceSummary
    {
        public string DocumentIssueDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        public string DocumentFormat { get; set; } = "JSON";
        public string DocumentVersion { get; set; } = "1.0";
        public string SubmissionUid { get; set; } = string.Empty;

        [JsonProperty("SubmissionDate")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime SubmissionDate { get; set; }
        public string DocumentUUID { get; set; } = string.Empty;
        public string DocumentLongId { get; set; } = string.Empty;
        public string DocumentStatus { get; set; } = string.Empty;
        public IntegrationType IntegrationType { get; set; }
        public string PublicUrl { get; set; } = string.Empty;

    }
}
