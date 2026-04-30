using Microsoft.EntityFrameworkCore;
using SafeCity.Domain.Data;
using SafeCity.Domain.Entity;
using SafeCity.Domain.Enum;
using SafeCity.DTOs.Patrol;
using PatrolEntity = SafeCity.Domain.Entity.Patrol;

namespace SafeCity.Repository.Patrol
{
    /// <summary>
    /// Handles all database operations for patrol management, including checking for duplicate schedules,
    /// saving new patrol records, and retrieving active police officers who are available on a given date.
    /// </summary>
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

        /// <summary>
        /// Retrieves a patrol by its unique identifier, or <c>null</c> if not found.
        /// </summary>
        public async Task<PatrolEntity?> GetByIdAsync(int patrolId)
        {
            return await _context.Patrols.FindAsync(patrolId);
        }

        public async Task<IEnumerable<PatrolEntity>> GetAllAsync(PatrolFilterDto filter)
        {
            var query = _context.Patrols.AsQueryable();

            if (filter.PatrolId.HasValue)
                query = query.Where(p => p.PatrolId == filter.PatrolId.Value);

            if (filter.OfficerId.HasValue)
                query = query.Where(p => p.OfficerId == filter.OfficerId.Value);

            if (filter.Date.HasValue)
                query = query.Where(p => p.Date.Date == filter.Date.Value.Date);

            return await query.OrderByDescending(p => p.Date).ToListAsync();
        }
    }
}
