
using Newtonsoft.Json;

namespace InvoisEGS.ApiClient.ApiModels
{
    public class SubmitDocumentResponse
    {
        [JsonProperty("statusCode")]  
        public System.Net.HttpStatusCode StatusCode { get; set; }

        [JsonProperty("submissionUID")]
        public string SubmissionUID { get; set; }

        [JsonProperty("acceptedDocuments")]
        public AcceptedDocument[] AcceptedDocuments { get; set; }

        [JsonProperty("rejectedDocuments")]
        public RejectedDocument[] RejectedDocuments { get; set; }
    }

    public class AcceptedDocument
    {
        [JsonProperty("uuid")]
        public string UUID { get; set; }

        [JsonProperty("invoiceCodeNumber")]
        public string InvoiceCodeNumber { get; set; }
    }

    public class RejectedDocument
    {
        [JsonProperty("invoiceCodeNumber")]
        public string InvoiceCodeNumber { get; set; }

        [JsonProperty("error")]
        public ErrorInfo Error { get; set; }
    }

    public class ErrorInfo
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
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