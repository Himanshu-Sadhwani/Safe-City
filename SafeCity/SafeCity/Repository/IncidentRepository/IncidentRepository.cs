using SafeCity.Domain.Data;
using SafeCity.DTOs.Incidents;

namespace SafeCity.Repository.IncidentRepository
{
    public class IncidentRepository : IIncidentRepository
    {
        // dependency injection
        private readonly SafeCityDbContext _safeCityDbContext;

        public IncidentRepository(SafeCityDbContext safeCityDbContext)
        {
            _safeCityDbContext = safeCityDbContext;
        }

        // incident submission repository layer logic
        public async Task SubmitIncident(IncidentCreateRequest request)
        {
            try
            {
                // map dto to the entity
                var incidentDetails = request.ToEntity();
                await _safeCityDbContext.Incidents.AddAsync(incidentDetails);
                await _safeCityDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // throws error if any
                throw new Exception("Database error occurred while saving the incident.", ex);
            }
        }
    }
}