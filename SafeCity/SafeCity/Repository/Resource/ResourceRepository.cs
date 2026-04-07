using Microsoft.EntityFrameworkCore;
using SafeCity.Domain.Entity;
using SafeCity.Domain.Enum;
using SafeCity.Domain.Data;
using SafeCity.Repository;

namespace SafeCity.Repository
{
    public class ResourceRepository : IResourceRepository
    {
        private readonly SafeCityDbContext _context;

        public ResourceRepository(SafeCityDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Resource>> GetAvailableResourcesAsync(ResourceTypeOption type)
        {
            var typeValue = ((int)type).ToString();
            var availableValue = ((int)ResourceAvailabilityOption.Available).ToString();

            return await _context.Resources
                .Where(r => r.Type.ToString() == typeValue &&
                            r.Availability.ToString() == availableValue)
                .ToListAsync();
        }

        public async Task UpdateAsync(Resource resource)
        {
            _context.Resources.Update(resource);
            await _context.SaveChangesAsync();
        }
    }
}