using SafeCity.Domain.Enum;
using SafeCity.DTOs.Incidents;

namespace SafeCity.Services.IncidentService
{
    public interface IIncidentService
    {
        public Task SubmitIncident(IncidentCreateRequest request);
        public Task<List<IncidentResponse>> ViewIncident(int userId, bool isAdmin, IncidentStatusOption? status, string? location, IncidentOption? type, DateTime? date);
    }
}
