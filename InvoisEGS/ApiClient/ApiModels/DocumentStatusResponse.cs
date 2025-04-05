using Newtonsoft.Json;

namespace InvoisEGS.ApiClient.ApiModels
{
    public class DocumentStatusResponse
    {
        [JsonProperty("submissionUid")]
        public string SubmissionUid { get; set; }

        [JsonProperty("documentCount")]
        public int DocumentCount { get; set; }

        [JsonProperty("dateTimeReceived")]
        public DateTime DateTimeReceived { get; set; }

        [JsonProperty("overallStatus")]
        public string OverallStatus { get; set; }

        [JsonProperty("documentSummary")]
        public DocumentSummary[] DocumentSummary { get; set; }
    }

    public class DocumentSummary
    {
        [JsonProperty("uuid")]
        public string UUID { get; set; }

        [JsonProperty("submissionUid")]
        public string SubmissionUid { get; set; }

        [JsonProperty("longId")]
        public string LongId { get; set; }

        [JsonProperty("internalId")]
        public string InternalId { get; set; }

        [JsonProperty("typeName")]
        public string TypeName { get; set; }

        [JsonProperty("typeVersionName")]
        public string TypeVersionName { get; set; }

        [JsonProperty("SupplierTin")]
        public string SupplierTin { get; set; }

        [JsonProperty("SupplierName")]
        public string SupplierName { get; set; }

        [JsonProperty("receiverId")]
        public string ReceiverId { get; set; }

        [JsonProperty("receiverName")]
        public string ReceiverName { get; set; }

        [JsonProperty("dateTimeIssued")]
        public DateTime DateTimeIssued { get; set; }

        [JsonProperty("dateTimeReceived")]
        public DateTime DateTimeReceived { get; set; }

        //[JsonProperty("dateTimeValidated")]
        //public DateTime DateTimeValidated { get; set; }

        [JsonProperty("totalExcludingTax")]
        public decimal TotalExcludingTax { get; set; }

        [JsonProperty("totalDiscount")]
        public decimal TotalDiscount { get; set; }

        [JsonProperty("totalNetAmount")]
        public decimal TotalNetAmount { get; set; }

        [JsonProperty("totalPayableAmount")]
        public decimal TotalPayableAmount { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("cancelDateTime")]
        public DateTime? CancelDateTime { get; set; }

        [JsonProperty("rejectRequestDateTime")]
        public DateTime? RejectRequestDateTime { get; set; }

        [JsonProperty("documentStatusReason")]
        public string DocumentStatusReason { get; set; }

        [JsonProperty("createdByUserId")]
        public string CreatedByUserId { get; set; }
    }
}