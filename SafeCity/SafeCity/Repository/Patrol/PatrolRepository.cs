using Microsoft.EntityFrameworkCore;
using SafeCity.Domain.Data;
using SafeCity.Domain.Entity;
using SafeCity.Domain.Enum;
using PatrolEntity = SafeCity.Domain.Entity.Patrol;

namespace SafeCity.Repository.Patrol
{
    public class PatrolRepository : IPatrolRepository
    {
        private readonly SafeCityDbContext _context;

        public PatrolRepository(SafeCityDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(int officerId, DateTime date)
        {
            return await _context.Patrols.AnyAsync(p => p.OfficerId == officerId && p.Date.Date == date.Date);
        }

        public async Task<PatrolEntity> AddAsync(PatrolEntity patrol)
        {
            await _context.Patrols.AddAsync(patrol);
            await _context.SaveChangesAsync();
            return patrol;
        }

        public async Task<List<User>> GetAvailableOfficersAsync(DateTime date)
        {
            var assignedOfficerIds = await _context.Patrols
                .Where(p => p.Date.Date == date.Date)
                .Select(p => p.OfficerId)
                .ToListAsync();

            return await _context.Users
                .Include(u => u.UserRole)
                .Where(u => u.UserRole.RoleName == UserRoleOption.Police
                         && u.Status == UserStatus.Active
                         && !assignedOfficerIds.Contains(u.UserID))
                .ToListAsync();
        }
    }
}
