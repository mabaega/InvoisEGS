using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Globalization;

namespace InvoisEGS.ApiClient.ApiHelpers
{
    public class CustomDateTimeConverter : IsoDateTimeConverter
    {
        public CustomDateTimeConverter()
        {
            DateTimeFormat = "yyyy-MM-ddTHH:mm:ssZ";
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is DateTime dateTime)
            {
                writer.WriteValue(dateTime.ToString(DateTimeFormat, CultureInfo.InvariantCulture));
            }
            else
            {
                base.WriteJson(writer, value, serializer);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                if (DateTime.TryParseExact((string)reader.Value, DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
                {
                    return dateTime;
                }
            }
            return base.ReadJson(reader, objectType, existingValue, serializer);
        }
    }
}
