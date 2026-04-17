using SafeCity.Domain.Entity;
using SafeCity.Domain.Enum;
using SafeCity.DTOs.Incidents;

namespace SafeCity.Repository
{
    public interface IIncidentRepository
    {
        public Task<Incident?> GetByIdAsync(int incidentId);
        public Task UpdateAsync(Incident incident);
        public Task SubmitIncident(IncidentCreateRequest request);
        public Task<List<IncidentResponse>> ViewIncident(int userId, bool isAdmin, IncidentStatusOption? status, string? location, IncidentOption? type, DateTime? date);
    }
}