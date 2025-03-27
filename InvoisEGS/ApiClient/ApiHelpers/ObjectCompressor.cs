using Newtonsoft.Json;
using System.IO.Compression;
using System.Text;

namespace InvoisEGS.ApiClient.ApiHelpers
{
    public static class ObjectCompressor
    {
        public static string? SerializeToBase64String(object obj)
        {
            try
            {
                string jsonString = JsonConvert.SerializeObject(obj);
                byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonString);

                byte[] compressedBytes = Compress(jsonBytes);

                return Convert.ToBase64String(compressedBytes);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static T? DeserializeFromBase64String<T>(string base64String)
        {
            try
            {
                byte[] compressedBytes = Convert.FromBase64String(base64String);

                byte[] jsonBytes = Decompress(compressedBytes);

                string jsonString = Encoding.UTF8.GetString(jsonBytes);

                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch (Exception)
            {
                return default;
            }
        }

        private static byte[] Compress(byte[] data)
        {
            using MemoryStream outputStream = new();
            using (BrotliStream brotliStream = new(outputStream, CompressionMode.Compress))
            {
                brotliStream.Write(data, 0, data.Length);
            }
            return outputStream.ToArray();
        }

        private static byte[] Decompress(byte[] data)
        {
            using MemoryStream inputStream = new(data);
            using MemoryStream outputStream = new();
            using (BrotliStream brotliStream = new(inputStream, CompressionMode.Decompress))
            {
                brotliStream.CopyTo(outputStream);
            }
            return outputStream.ToArray();
        }
    }
}
