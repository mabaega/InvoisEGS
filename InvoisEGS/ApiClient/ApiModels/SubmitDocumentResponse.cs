using System.Text.Json.Serialization;

namespace InvoisEGS.ApiClient.ApiModels
{
    public class SubmitDocumentResponse
    {
        [JsonPropertyName("statusCode")]  
        public System.Net.HttpStatusCode StatusCode { get; set; }

        [JsonPropertyName("submissionUID")]
        public string SubmissionUID { get; set; }

        [JsonPropertyName("acceptedDocuments")]
        public AcceptedDocument[] AcceptedDocuments { get; set; }

        [JsonPropertyName("rejectedDocuments")]
        public RejectedDocument[] RejectedDocuments { get; set; }
    }

    public class AcceptedDocument
    {
        [JsonPropertyName("uuid")]
        public string UUID { get; set; }

        [JsonPropertyName("invoiceCodeNumber")]
        public string InvoiceCodeNumber { get; set; }
    }

    public class RejectedDocument
    {
        [JsonPropertyName("invoiceCodeNumber")]
        public string InvoiceCodeNumber { get; set; }

        [JsonPropertyName("error")]
        public ErrorInfo Error { get; set; }
    }

    public class ErrorInfo
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        public static class ErrorCodes
        {
            public const string BadStructure = "BadStructure";
            public const string MaximumSizeExceeded = "MaximumSizeExceeded";
            public const string IncorrectSubmitter = "IncorrectSubmitter";
            public const string DuplicateSubmission = "DuplicateSubmission";
        }
    }
}