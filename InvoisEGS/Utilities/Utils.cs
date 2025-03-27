using Newtonsoft.Json;
using System.Globalization;
using System.Text;

namespace InvoisEGS.Utilities
{
    public static class Utils
    {
        public static string ConstructInvoiceApiUrl(string referrer, string invoiceUUID)
        {
            Uri uri = new(referrer);
            string baseUrl = $"{uri.Scheme}://{uri.Host}";

            if (uri.Port is not 80 and not 443)
            {
                baseUrl += $":{uri.Port}";
            }

            if (referrer.Contains("purchase-invoice-view"))
            {
                return $"{baseUrl}/api2/purchase-invoice-form/{invoiceUUID}";
            }
            else if (referrer.Contains("sales-invoice-view"))
            {
                return $"{baseUrl}/api2/sales-invoice-form/{invoiceUUID}";
            }
            else if (referrer.Contains("debit-note-view"))
            {
                return $"{baseUrl}/api2/debit-note-form/{invoiceUUID}";
            }
            else if (referrer.Contains("credit-note-view"))
            {
                return $"{baseUrl}/api2/credit-note-form/{invoiceUUID}";
            }

            throw new ArgumentException("Invalid referrer URL");
        }

        public static string SerializeObject<T>(T value)
        {
            StringBuilder sb = new(256);
            StringWriter sw = new(sb, CultureInfo.InvariantCulture);

            JsonSerializer jsonSerializer = JsonSerializer.CreateDefault();
            using (JsonTextWriter jsonWriter = new(sw))
            {
                jsonWriter.Formatting = Formatting.Indented;
                jsonWriter.IndentChar = ' ';
                jsonWriter.Indentation = 4;

                jsonSerializer.Serialize(jsonWriter, value, typeof(T));
            }

            return sw.ToString();
        }
        public static string GetValue(Dictionary<string, string> formData, string key)
        {
            return formData.GetValueOrDefault(key) ?? string.Empty;
        }

    }
}
