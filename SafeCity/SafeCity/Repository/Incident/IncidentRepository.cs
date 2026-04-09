using Microsoft.EntityFrameworkCore;
using SafeCity.Domain.Data;
using SafeCity.Domain.Entity;
using SafeCity.DTOs.Incidents;

namespace SafeCity.Repository
{
    /// <summary>
    /// Repository responsible for handling data access operations
    /// related to incidents.
    /// </summary>
    public class IncidentRepository : IIncidentRepository
    {
        private readonly SafeCityDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="IncidentRepository"/> class.
        /// </summary>
        /// <param name="context">
        /// The database context used to access incident data.
        /// </param>
        public IncidentRepository(SafeCityDbContext context)
        {
            _context = context;
        }
        public async Task SubmitIncident(IncidentCreateRequest request)
        {
            try
            {
                // map dto to the entity
                var incidentDetails = request.ToEntity();
                await _context.Incidents.AddAsync(incidentDetails);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // throws error if any
                throw new Exception("Database error occurred while saving the incident.", ex);
            }
        }

        /// <summary>
        /// Retrieves an incident by its unique identifier.
        /// </summary>
        /// <param name="incidentId">The unique identifier of the incident.</param>
        /// <returns>
        /// The <see cref="Incident"/> if found; otherwise <c>null</c>.
        /// </returns>
        public async Task<Incident?> GetByIdAsync(int incidentId)
        {
            return await _context.Incidents
                .AsNoTracking() 
                .FirstOrDefaultAsync(i => i.IncidentID == incidentId);
        }

        /// <summary>
        /// Updates an existing incident record in the database.
        /// </summary>
        /// <param name="incident">The incident entity to update.</param>
        /// <returns>A task representing the asynchronous update operation.</returns>
        public async Task UpdateAsync(Incident incident)
        {
            _context.Incidents.Update(incident);
            await _context.SaveChangesAsync();
        }

    }
}