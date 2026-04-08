using SafeCity.DTOs.Incidents;

namespace SafeCity.Services.IncidentService
{
    public interface IIncidentService
    {
        public Task SubmitIncident(IncidentCreateRequest request);
    }
}
