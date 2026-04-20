using Microsoft.EntityFrameworkCore;
using SafeCity.Domain.Entity;
using SafeCity.Domain.Enum;
using SafeCity.Domain.Data;

namespace SafeCity.Repository
{
    /// <summary>
    /// Repository responsible for managing resource-related data operations.
    /// </summary>
    public class ResourceRepository : IResourceRepository
    {
        private readonly SafeCityDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceRepository"/> class.
        /// </summary>
        /// <param name="context">The database context used to access resource data.</param>
        public ResourceRepository(SafeCityDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all available resources of the specified type.
        /// </summary>
        /// <param name="type">The type of resource to retrieve.</param>
        /// <returns>
        /// A task that returns a collection of available <see cref="Resource"/> entities
        /// matching the specified resource type.
        /// </returns>
        public async Task<IEnumerable<Resource>> GetAvailableResourcesAsync(ResourceTypeOption type)
        {
            return await _context.Resources
                .Where(r => r.Type == type &&
                            r.Availability == ResourceAvailabilityOption.Available)
                .ToListAsync();
        }

        /// <summary>
        /// Updates an existing resource record in the database.
        /// </summary>
        /// <param name="resource">The resource entity containing updated information.</param>
        /// <returns>A task that represents the asynchronous update operation.</returns>
        public async Task UpdateAsync(Resource resource)
        {
            var existing = await _context.Resources.FindAsync(resource.ResourceID);
            if (existing == null) return;

            existing.Type = resource.Type;
            existing.Availability = resource.Availability;

            await _context.SaveChangesAsync();
        }
    }
}