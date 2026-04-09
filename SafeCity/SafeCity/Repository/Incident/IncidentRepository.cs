using Microsoft.EntityFrameworkCore;
using SafeCity.Domain.Data;
using SafeCity.Domain.Entity;
using SafeCity.Domain.Enum;
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

                // duplicate Incident Check
                var isDuplicate = await _context.Incidents.AnyAsync(i => i.CitizenID == request.CitizenID && i.Location.ToLower() == request.Location.ToLower() && i.Type == request.Type && i.Date.Date == request.Date.Date);

                if (isDuplicate)
                {

                    throw new InvalidOperationException("This incident has already been reported.");
                }
                // if duplicate Incident not found
                await _context.Incidents.AddAsync(incidentDetails);
                await _context.SaveChangesAsync();
            }
            catch (InvalidOperationException)
            {
                throw;
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


        // Repository layer logic to filter and fetch the Incident Details to perform the next Action by the Authorities
        public async Task<List<IncidentResponse>> ViewIncident(int userId, bool isAdmin, IncidentStatusOption? status, string? location, IncidentOption? type, DateTime? date)
        {
            try
            {
                // if request made by citizen
                if (isAdmin == false)
                {
                    // list all the incident logged by the logged in citizen
                    var incidents = await _context.Incidents
                        .Where(temp => temp.CitizenID == userId)
                        .ToListAsync();

                    // Filteration for the logged in user incident list
                    if (status.HasValue)
                    {
                        incidents = incidents.Where(temp => temp.Status == status.Value).ToList();
                    }
                    if (!string.IsNullOrEmpty(location))
                    {
                        incidents = incidents.Where(temp => temp.Location.Contains(location, StringComparison.OrdinalIgnoreCase)).ToList();
                    }
                    if (type.HasValue)
                    {
                        incidents = incidents.Where(temp => temp.Type == type.Value).ToList();
                    }
                    if (date.HasValue)
                    {
                        incidents = incidents.Where(temp => temp.Date.Date == date.Value.Date).ToList();
                    }

                    // returning the Incident Response DTO
                    return incidents.Select(temp => IncidentResponseExtension.ToIncidentResponse(temp)).ToList();
                }
                else
                {
                    // if the request is made by the admin
                    var incidents = await _context.Incidents.ToListAsync();

                    // admin can apply all the filteration on the incident that is logged by the incident
                    if (status.HasValue)
                    {
                        incidents = incidents.Where(temp => temp.Status == status.Value).ToList();
                    }
                    if (!string.IsNullOrEmpty(location))
                    {
                        incidents = incidents.Where(temp => temp.Location.Contains(location, StringComparison.OrdinalIgnoreCase)).ToList();
                    }
                    if (type.HasValue)
                    {
                        incidents = incidents.Where(temp => temp.Type == type.Value).ToList();
                    }
                    if (date.HasValue)
                    {
                        incidents = incidents.Where(temp => temp.Date.Date == date.Value.Date).ToList();
                    }

                    return incidents.Select(temp => IncidentResponseExtension.ToIncidentResponse(temp)).ToList();
                }
            }
            catch (Exception ex)
            {
                // throws errrors if any present
                throw new Exception(ex.Message);
            }
        }
    }
}