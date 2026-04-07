using Microsoft.EntityFrameworkCore;
using SafeCity.Domain.Data;
using SafeCity.Domain.Enum;
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

        // Repository Layer Logic to View all the Incident by the admin and the respective incident by a single individual Citizen can able to view his Incident.
        public async Task<List<IncidentResponse>> ViewIncident(int userId, bool isAdmin, int incidentStatusType)
        {
            try
            {
                // if the request is made by the citizen only
                if (isAdmin == false)
                {
                    // find the all the incident list reported by that logged in citizen
                    var filteredResponse = await _safeCityDbContext.Incidents
                        .Where(temp => temp.CitizenID == userId)
                        .ToListAsync();

                    return filteredResponse.Select(temp => IncidentResponseExtension.ToIncidentResponse(temp)).ToList();
                }
                else
                {
                    // if the request is made by the admin only
                    // based on the incidentStatus type result will be displayed
                    if (incidentStatusType == 0)
                    {
                        var filteredReponse = await _safeCityDbContext.Incidents
                            .Where(temp => temp.Status == IncidentStatusOption.Pending)
                            .ToListAsync();

                        return filteredReponse.Select(temp => IncidentResponseExtension.ToIncidentResponse(temp)).ToList();
                    }
                    if (incidentStatusType == 1)
                    {
                        var filteredReponse = await _safeCityDbContext.Incidents
                            .Where(temp => temp.Status == IncidentStatusOption.InProgress)
                            .ToListAsync();

                        return filteredReponse.Select(temp => IncidentResponseExtension.ToIncidentResponse(temp)).ToList();
                    }
                    if (incidentStatusType == 2)
                    {

                        var filteredReponse = await _safeCityDbContext.Incidents
                            .Where(temp => temp.Status == IncidentStatusOption.Resolved)
                            .ToListAsync();

                        return filteredReponse.Select(temp => IncidentResponseExtension.ToIncidentResponse(temp)).ToList();
                    }
                    else
                    {
                        // find the all the incident list 
                        var filteredResponse = await _safeCityDbContext.Incidents.ToListAsync();

                        return filteredResponse.Select(temp => IncidentResponseExtension.ToIncidentResponse(temp)).ToList();
                    }
                }
                return new List<IncidentResponse>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}