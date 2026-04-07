using SafeCity.DTOs.Incidents;

namespace SafeCity.Repository.IncidentRepository
{
    public interface IIncidentRepository
    {
        public Task SubmitIncident(IncidentCreateRequest request);
        public Task<List<IncidentResponse>> ViewIncident(int userId, bool isAdmin, int incidentStatusType);
    }
}
