using Microsoft.EntityFrameworkCore;
using SafeCity.Domain.Entity;
using SafeCity.Domain.Enum;
using SafeCity.Domain.Data;
using SafeCity.DTOs.Resource;

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

        public async Task<List<GetResourceResponseDto>> ViewResources(
                    int? resourceId,
                    ResourceTypeOption? type,
                    ResourceAvailabilityOption? availability,
                    string? location,
                    string? sortOrder)
        {
            try
            {
                var query = _context.Resources.AsQueryable();

                if (resourceId.HasValue)
                    query = query.Where(r => r.ResourceID == resourceId.Value);

                if (type.HasValue)
                    query = query.Where(r => r.Type == type.Value);

                if (availability.HasValue)
                    query = query.Where(r => r.Availability == availability.Value);

                if (!string.IsNullOrWhiteSpace(location))
                    query = query.Where(r =>
                        r.Location.Contains(location));

                query = string.Equals(sortOrder, "desc", StringComparison.OrdinalIgnoreCase)
                    ? query.OrderByDescending(r => r.ResourceID)
                    : query.OrderBy(r => r.ResourceID);

                return await query.Select(r => new GetResourceResponseDto
                {
                    ResourceID = r.ResourceID,
                    Type = r.Type,
                    Availability = r.Availability,
                    Location = r.Location,
                    UnitName = r.UnitName
                }).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> UnitExistsAsync(string unitName)
        {
            return await _context.Resources
                .AnyAsync(r => r.UnitName == unitName);
        }
    }
}