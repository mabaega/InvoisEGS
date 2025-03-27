using MyInvois.ApiClient.Models;

public interface IApiHelper
{
    Task<TokenInfo> GetAccessTokenAsync();
    Task<SubmitDocumentResponse> SubmitDocumentAsync(SubmitDocumentRequest submitRequest);
    Task<DocumentStatusResponse> GetDocumentStatusAsync(string submissionUid, int pageNo = 1, int pageSize = 10);
    Task<(SubmitDocumentResponse Submit, DocumentStatusResponse Status)> SubmitAndMonitorAsync(
        SubmitDocumentRequest submitRequest,
        CancellationToken cancellationToken = default);

    // Method baru untuk polling status dengan progress callback
    Task<DocumentStatusResponse> WaitForCompletionAsync(
        string submissionUid,
        Action<DocumentStatusResponse> onProgress = null,
        CancellationToken cancellationToken = default);
}