using System;
using SafeCity.Domain.Data;
using SafeCity.Domain.Entity;
using Microsoft.EntityFrameworkCore;

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
    }
}