namespace InvoisEGS.Models
{
    public class ApprovedInvoiceViewModel
    {
        public string Referrer { get; set; }
        public string SubmitDocumentRequestJson { get; set; } = "{}"; //sementara biarkan kosong
        public string ServerResponseJson { get; set; } = "{}"; //sementara biarkan kosong
        public InvoiceSummary InvoiceSummary { get; internal set; }
        public ApprovedInvoiceViewModel() { }
    }
}