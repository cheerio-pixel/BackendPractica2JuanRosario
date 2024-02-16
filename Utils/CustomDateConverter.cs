
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace backend.Utils
{
    public class CustomDateConverter : JsonConverter<DateOnly>
    {
        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                if (DateOnly.TryParseExact(reader.GetString(), "yyyy/MM/dd", null, System.Globalization.DateTimeStyles.None, out DateOnly result))
                {
                    return result;
                }
            }

            return DateOnly.FromDateTime(reader.GetDateTime());
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy/MM/dd"));
        }
    }

}