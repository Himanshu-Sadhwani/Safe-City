using System.Text.Json;
using System.Text.Json.Serialization;

namespace SafeCity.Utility
{
    public class PatrolValidationHelper : JsonConverter<int?>
    {
        public override int? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return null;

            if (reader.TokenType == JsonTokenType.Number && reader.TryGetInt32(out int value))
                return value;

            // Non-integer value (string, bool, float, etc.) — consume token and return sentinel
            // [Range] validation will catch -1 and return "Enter valid officer id"
            using var doc = JsonDocument.ParseValue(ref reader);
            return -1;
        }

        public override void Write(Utf8JsonWriter writer, int? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
                writer.WriteNumberValue(value.Value);
            else
                writer.WriteNullValue();
        }
    }
}
