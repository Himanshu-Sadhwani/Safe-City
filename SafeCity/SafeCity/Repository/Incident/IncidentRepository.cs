using Microsoft.EntityFrameworkCore;
using SafeCity.Domain.Data;
using SafeCity.Domain.Entity;
using SafeCity.Repository;

namespace SafeCity.Repository
{
    public class IncidentRepository : IIncidentRepository
    {
        private readonly SafeCityDbContext _context;

        public IncidentRepository(SafeCityDbContext context)
        {
            _context = context;
        }

        public async Task<Incident?> GetByIdAsync(int incidentId)
        {
            return await _context.Incidents
                .FirstOrDefaultAsync(i => i.IncidentID == incidentId);
        }

        public async Task UpdateAsync(Incident incident)
        {
            _context.Incidents.Update(incident);
            await _context.SaveChangesAsync();
        }
    }
}