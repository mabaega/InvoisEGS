namespace InvoisEGS.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }
        public string ErrorMessage { get; set; }
        public string Referrer { get; set; }
        public List<string> ValidationErrors { get; set; } = new List<string>();
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
