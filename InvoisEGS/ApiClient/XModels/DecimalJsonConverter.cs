using Newtonsoft.Json;
using System.Globalization;

namespace InvoisEGS.ApiClient.XModels
{
    public class DecimalJsonConverter : JsonConverter<decimal>
    {
        public DecimalJsonConverter()
        {
        }

        public override decimal ReadJson(JsonReader reader, Type objectType, decimal existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.Value == null) return 0;
            return Convert.ToDecimal(reader.Value);
        }

        public override void WriteJson(JsonWriter writer, decimal value, JsonSerializer serializer)
        {
            string formatted = value.ToString("0.##########", CultureInfo.InvariantCulture);
            writer.WriteValue(formatted); // Gunakan WriteValue agar tetap valid JSON
                                          //writer.WriteRawValue(formatted);
        }
    }
}
