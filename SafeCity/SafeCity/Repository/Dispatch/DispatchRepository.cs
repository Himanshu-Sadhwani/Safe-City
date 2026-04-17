using System;
using SafeCity.Domain.Data;
using SafeCity.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using SafeCity.DTOs.Dispatch;
using SafeCity.Domain.Enum;

namespace SafeCity.Repository
{
    /// <summary>
    /// Repository responsible for managing Dispatch-related data operations.
    /// </summary>
    public class DispatchRepository : IDispatchRepository
    {
        private readonly SafeCityDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="DispatchRepository"/> class.
        /// </summary>
        /// <param name="context">The database context used for accessing dispatch data.</param>
        public DispatchRepository(SafeCityDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new dispatch record to the database.
        /// </summary>
        /// <param name="dispatch">The dispatch entity to be added.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        
        public async Task AddAsync(Dispatch dispatch)
        {
            await _context.Dispatches.AddAsync(dispatch);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Checks whether a dispatch already exists for a given incident.
        /// </summary>
        /// <param name="incidentId">The unique identifier of the incident.</param>
        /// <returns>
        /// A task that returns <c>true</c> if a dispatch exists for the specified incident;
        /// otherwise, <c>false</c>.
        /// </returns>
        public async Task<List<Dispatch>> GetByIncidentIdAsync(int incidentId)
        {
            return await _context.Dispatches
                .Where(d => d.IncidentID == incidentId)
                .ToListAsync();
        }
        public async Task<Domain.Entity.Dispatch?> GetByIdAsync(int dispatchId)
        {
            return await _context.Dispatches
                .FirstOrDefaultAsync(d => d.DispatchID == dispatchId);
        }

        public async Task UpdateAsync(Dispatch dispatch)
        {
            _context.Dispatches.Update(dispatch);
            await _context.SaveChangesAsync();
        }
        public async Task<List<GetResponseDto>> ViewDispatch(
            int? incidentId,
            bool isAdmin,
            int? resourceId,
            int? dispatcherId,
            DispatchStatusOption? status,
            DateTime? date,
            string? sortOrder)
        {
            try
            {
                // if request is NOT made by admin
                if (isAdmin == false)
                {
                    // non-admin can see only their dispatches
                    var dispatches = await _context.Dispatches
                        .Include(d => d.User)
                        .Include(d => d.Resource)
                        .Include(d => d.Incident)
                        .Where(d => d.DispatcherID == dispatcherId)
                        .ToListAsync();

                    // Apply filters
                    if (incidentId.HasValue)
                    {
                        dispatches = dispatches
                            .Where(d => d.IncidentID == incidentId.Value)
                            .ToList();
                    }

                    if (resourceId.HasValue)
                    {
                        dispatches = dispatches
                            .Where(d => d.ResourceID == resourceId.Value)
                            .ToList();
                    }

                    if (status.HasValue)
                    {
                        dispatches = dispatches
                            .Where(d => d.Status == status.Value)
                            .ToList();
                    }

                    if (date.HasValue)
                    {
                        dispatches = dispatches
                            .Where(d => d.Date.Date == date.Value.Date)
                            .ToList();
                    }

                    // Latest dispatch first
                    dispatches = dispatches
                        .OrderBy(d => d.DispatchID)
                        .ToList();

                    return dispatches.Select(d => new GetResponseDto
                    {
                        DispatchID = d.DispatchID,
                        IncidentId = d.IncidentID,
                        DispatcherId = d.DispatcherID,
                        DispatcherName = d.User != null ? d.User.Name : string.Empty,
                        ResourceId =d.ResourceID,
                        Date = d.Date,
                        Status = d.Status
                    }).ToList();
                }
                else
                {
                    // if request is made by admin
                    var dispatches = await _context.Dispatches
                        .Include(d => d.User)
                        .Include(d => d.Resource)
                        .Include(d => d.Incident)
                        .ToListAsync();

                    // Apply filters
                    if (incidentId.HasValue)
                    {
                        dispatches = dispatches
                            .Where(d => d.IncidentID == incidentId.Value)
                            .ToList();
                    }

                    if (resourceId.HasValue)
                    {
                        dispatches = dispatches
                            .Where(d => d.ResourceID == resourceId.Value)
                            .ToList();
                    }

                    if (dispatcherId.HasValue)
                    {
                        dispatches = dispatches
                            .Where(d => d.DispatcherID == dispatcherId.Value)
                            .ToList();
                    }

                    if (status.HasValue)
                    {
                        dispatches = dispatches
                            .Where(d => d.Status == status.Value)
                            .ToList();
                    }

                    if (date.HasValue)
                    {
                        dispatches = dispatches
                            .Where(d => d.Date.Date == date.Value.Date)
                            .ToList();
                    }

                    // Latest dispatch first
                    
                    
                    dispatches = string.Equals(sortOrder, "desc", StringComparison.OrdinalIgnoreCase)
                        ? dispatches.OrderByDescending(d => d.DispatchID).ToList()
                        : dispatches.OrderBy(d => d.DispatchID).ToList();


                    return dispatches.Select(d => new GetResponseDto
                    {
                        DispatchID = d.DispatchID,
                        IncidentId = d.IncidentID,
                        DispatcherId = d.DispatcherID,
                        DispatcherName = d.User != null ? d.User.Name : string.Empty,
                        ResourceId = d.ResourceID,
                        Date = d.Date,
                        Status = d.Status
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                // throws errors if any present
                throw new Exception(ex.Message);
            }
        }
    }
}