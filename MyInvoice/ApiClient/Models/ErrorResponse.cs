using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyInvois.ApiClient.Models
{
    public class ErrorResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("error")]
        public Error Error { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("statusCode")]
        public HttpStatusCode StatusCode { get; set; }

        [JsonPropertyName("errorCode")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ApiErrorCode ErrorCode { get; set; }

        public static async Task<ErrorResponse> HandleResponseAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return null;
            }

            ErrorResponse errorResponse = await JsonSerializer.DeserializeAsync<ErrorResponse>(
                await response.Content.ReadAsStreamAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            if (errorResponse != null)
            {
                errorResponse.StatusCode = response.StatusCode;
                errorResponse.ErrorCode = MapStatusCodeToErrorCode(response.StatusCode);
            }

            return errorResponse;
        }

        private static ApiErrorCode MapStatusCodeToErrorCode(HttpStatusCode statusCode)
        {
            return statusCode switch
            {
                HttpStatusCode.BadRequest => ApiErrorCode.BadRequest,
                HttpStatusCode.Unauthorized => ApiErrorCode.Unauthorized,
                HttpStatusCode.Forbidden => ApiErrorCode.Forbidden,
                HttpStatusCode.NotFound => ApiErrorCode.NotFound,
                HttpStatusCode.TooManyRequests => ApiErrorCode.TooManyRequests,
                HttpStatusCode.NotImplemented => ApiErrorCode.NotImplemented,
                HttpStatusCode.ServiceUnavailable => ApiErrorCode.ServiceUnavailable,
                _ => ApiErrorCode.InternalServerError
            };
        }
    }

    public enum ApiErrorCode
    {
        BadRequest,
        BadArgument,
        Unauthorized,
        Forbidden,
        NotFound,
        TooManyRequests,
        InternalServerError,
        NotImplemented,
        ServiceUnavailable
    }

    public class Error
    {
        [JsonPropertyName("propertyName")]
        public string PropertyName { get; set; }

        [JsonPropertyName("propertyPath")]
        public string PropertyPath { get; set; }

        [JsonPropertyName("errorCode")]
        public string ErrorCode { get; set; }

        [JsonPropertyName("error")]
        public string ErrorMessage { get; set; }

        [JsonPropertyName("errorMS")]
        public string ErrorMessageMS { get; set; }

        [JsonPropertyName("innerError")]
        public ErrorDetail[] InnerErrors { get; set; }

        [JsonPropertyName("retryAfter")]
        public int? RetryAfter { get; set; }
    }

    public class ErrorDetail
    {
        [JsonPropertyName("propertyName")]
        public string PropertyName { get; set; }

        [JsonPropertyName("propertyPath")]
        public string PropertyPath { get; set; }

        [JsonPropertyName("errorCode")]
        public string ErrorCode { get; set; }

        [JsonPropertyName("error")]
        public string ErrorMessage { get; set; }

        [JsonPropertyName("errorMs")]
        public string ErrorMessageMS { get; set; }

        [JsonPropertyName("innerError")]
        public object InnerError { get; set; }
    }
}