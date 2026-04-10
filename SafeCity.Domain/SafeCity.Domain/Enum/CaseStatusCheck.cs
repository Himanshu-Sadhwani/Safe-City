using System.Text.Json.Serialization;

namespace SafeCity.Domain.Enum
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CaseStatusCheck
    {
        Open = 1, Closed = 2, Under_Investigation = 3
    }
}
