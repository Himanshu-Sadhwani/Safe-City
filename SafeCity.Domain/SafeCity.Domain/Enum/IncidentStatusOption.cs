using System.Text.Json.Serialization;

namespace SafeCity.Domain.Enum
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum IncidentStatusOption
    {
        Pending = 1, InProgress = 2, Resolved = 3
    }
}
