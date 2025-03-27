using System.Text.Json.Serialization;

namespace InvoisEGS.ApiClient.ApiModels
{
    public class DocumentStatusResponse
    {
        [JsonPropertyName("submissionUid")]
        public string SubmissionUid { get; set; }

        [JsonPropertyName("documentCount")]
        public int DocumentCount { get; set; }

        [JsonPropertyName("dateTimeReceived")]
        public DateTime DateTimeReceived { get; set; }

        [JsonPropertyName("overallStatus")]
        public string OverallStatus { get; set; }

        [JsonPropertyName("documentSummary")]
        public DocumentSummary[] DocumentSummary { get; set; }
    }

    public class DocumentSummary
    {
        [JsonPropertyName("uuid")]
        public string UUID { get; set; }

        [JsonPropertyName("submissionUid")]
        public string SubmissionUid { get; set; }

        [JsonPropertyName("longId")]
        public string LongId { get; set; }

        [JsonPropertyName("internalId")]
        public string InternalId { get; set; }

        [JsonPropertyName("typeName")]
        public string TypeName { get; set; }

        [JsonPropertyName("typeVersionName")]
        public string TypeVersionName { get; set; }

        [JsonPropertyName("SupplierTin")]
        public string SupplierTin { get; set; }

        [JsonPropertyName("SupplierName")]
        public string SupplierName { get; set; }

        [JsonPropertyName("receiverId")]
        public string ReceiverId { get; set; }

        [JsonPropertyName("receiverName")]
        public string ReceiverName { get; set; }

        [JsonPropertyName("dateTimeIssued")]
        public DateTime DateTimeIssued { get; set; }

        [JsonPropertyName("dateTimeReceived")]
        public DateTime DateTimeReceived { get; set; }

        [JsonPropertyName("dateTimeValidated")]
        public DateTime DateTimeValidated { get; set; }

        [JsonPropertyName("totalExcludingTax")]
        public decimal TotalExcludingTax { get; set; }

        [JsonPropertyName("totalDiscount")]
        public decimal TotalDiscount { get; set; }

        [JsonPropertyName("totalNetAmount")]
        public decimal TotalNetAmount { get; set; }

        [JsonPropertyName("totalPayableAmount")]
        public decimal TotalPayableAmount { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("cancelDateTime")]
        public DateTime? CancelDateTime { get; set; }

        [JsonPropertyName("rejectRequestDateTime")]
        public DateTime? RejectRequestDateTime { get; set; }

        [JsonPropertyName("documentStatusReason")]
        public string DocumentStatusReason { get; set; }

        [JsonPropertyName("createdByUserId")]
        public string CreatedByUserId { get; set; }
    }
}