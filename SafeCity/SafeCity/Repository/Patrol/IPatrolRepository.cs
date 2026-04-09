using SafeCity.Domain.Entity;
using PatrolEntity = SafeCity.Domain.Entity.Patrol;

namespace SafeCity.Repository.Patrol
{
    public interface IPatrolRepository
    {
        Task<bool> ExistsAsync(int officerId, DateTime date);
        Task<PatrolEntity> AddAsync(PatrolEntity patrol);
        Task<List<User>> GetAvailableOfficersAsync(DateTime date);
    }
}
