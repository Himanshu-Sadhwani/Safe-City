using SafeCity.Domain.Enum;
using SafeCity.DTOs.Patrol;
using SafeCity.Repository;
using SafeCity.Repository.Patrol;
using SafeCity.Utility;

namespace SafeCity.Services.PatrolService
{
    public class PatrolService : IPatrolService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPatrolRepository _patrolRepository;

        public PatrolService(IUserRepository userRepository, IPatrolRepository patrolRepository)
        {
            _userRepository = userRepository;
            _patrolRepository = patrolRepository;
        }

        public async Task<List<AvailableOfficerDto>> GetAvailableOfficersAsync(DateTime date)
        {
            if (date.Date < DateTime.Today)
                throw new ArgumentException(ErrorMessages.Patrol.InvalidDate);

            var officers = await _patrolRepository.GetAvailableOfficersAsync(date);

            if (officers == null || !officers.Any())
                throw new KeyNotFoundException(ErrorMessages.Patrol.NoOfficersAvailable);

            return officers.Select(o => new AvailableOfficerDto
            {
                OfficerId = o.UserID,
                Name = o.Name,
                Email = o.Email,
                Phone = o.Phone
            }).ToList();
        }

        public async Task<CreatePatrolResponseDto> CreatePatrolAsync(CreatePatrolRequestDto requestDto)
        {
            var officer = await _userRepository.GetUserByIdAsync(requestDto.OfficerId!.Value);

            if (officer == null)
                throw new KeyNotFoundException(ErrorMessages.Patrol.OfficerNotFound);

            if (officer.UserRole?.RoleName != UserRoleOption.Police)
                throw new UnauthorizedAccessException(ErrorMessages.Patrol.NotPoliceOfficer);

            if (officer.Status != UserStatus.Active)
                throw new InvalidOperationException(ErrorMessages.Patrol.OfficerNotActive);

            if (requestDto.Date!.Value.Date < DateTime.Today)
                throw new ArgumentException(ErrorMessages.Patrol.PastDate);

            var exists = await _patrolRepository.ExistsAsync(requestDto.OfficerId.Value, requestDto.Date.Value);
            if (exists)
                throw new InvalidOperationException(ErrorMessages.Patrol.AlreadyScheduled);

            var patrol = new Domain.Entity.Patrol
            {
                OfficerId = requestDto.OfficerId.Value,
                Area = requestDto.Area!,
                Date = requestDto.Date.Value,
                Status = PatrolStatus.Active
            };

            var saved = await _patrolRepository.AddAsync(patrol);

            return new CreatePatrolResponseDto
            {
                PatrolId = saved.PatrolId,
                OfficerId = saved.OfficerId,
                Area = saved.Area,
                Date = saved.Date,
                Status = saved.Status.ToString()
            };
        }

        public async Task<IEnumerable<PatrolResponseDto>> GetAllAsync(PatrolFilterDto filter)
        {
            // Validate PatrolId if provided
            if (filter.PatrolId.HasValue)
            {
                var patrolExists = await _patrolRepository.GetByIdAsync(filter.PatrolId.Value);
                if (patrolExists is null)
                    throw new KeyNotFoundException("no such Patrol Id");
            }

            // Validate OfficerId if provided
            if (filter.OfficerId.HasValue)
            {
                var officer = await _userRepository.GetUserByIdAsync(filter.OfficerId.Value);
                if (officer is null)
                    throw new KeyNotFoundException("no such Officer Id");
            }

            var patrols = await _patrolRepository.GetAllAsync(filter);

            if (!patrols.Any() && (filter.PatrolId.HasValue || filter.OfficerId.HasValue || filter.Date.HasValue))
                throw new KeyNotFoundException("Patrol does not exist for required fields");

            return patrols.Select(p => new PatrolResponseDto
            {
                PatrolId = p.PatrolId,
                OfficerId = p.OfficerId,
                Area = p.Area,
                Date = p.Date,
                Status = p.Date.Date == DateTime.Today ? "Active"
                       : p.Date.Date < DateTime.Today ? "Completed"
                       : "Upcoming"
            });
        }
    }
}
