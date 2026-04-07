using SafeCity.DTOs.Incidents;

namespace SafeCity.Services.IncidentService
{
    public interface IIncidentService
    {
        public Task SubmitIncident(IncidentCreateRequest request);
        public Task<List<IncidentResponse>> ViewIncident(int userId, bool isAdmin, int incidentStatusType);
    }
}
