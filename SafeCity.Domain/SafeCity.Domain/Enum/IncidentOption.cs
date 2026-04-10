using System.Text.Json.Serialization;

namespace SafeCity.Domain.Enum
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum IncidentOption
    {
        Crime = 1, Fire = 2, Accident = 3, Other = 4
    }
}
