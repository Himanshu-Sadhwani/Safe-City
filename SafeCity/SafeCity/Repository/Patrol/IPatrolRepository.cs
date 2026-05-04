using SafeCity.Domain.Entity;
using SafeCity.DTOs.Patrol;
using PatrolEntity = SafeCity.Domain.Entity.Patrol;

namespace SafeCity.Repository.Patrol
{
    public interface IPatrolRepository
    {
        Task<bool> ExistsAsync(int officerId, DateTime date);

        Task<PatrolEntity> AddAsync(PatrolEntity patrol);

        Task<List<User>> GetAvailableOfficersAsync(DateTime date);

        Task<PatrolEntity?> GetByIdAsync(int patrolId);

        Task<IEnumerable<PatrolEntity>> GetAllAsync(PatrolFilterDto filter);
    }
}
