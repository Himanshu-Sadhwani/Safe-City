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
            var officer = await _userRepository.GetUserByIdAsync(requestDto.OfficerId);

            if (officer == null)
                throw new KeyNotFoundException(ErrorMessages.Patrol.OfficerNotFound);

            if (officer.UserRole?.RoleName != UserRoleOption.Police)
                throw new UnauthorizedAccessException(ErrorMessages.Patrol.NotPoliceOfficer);

            if (officer.Status != UserStatus.Active)
                throw new InvalidOperationException(ErrorMessages.Patrol.OfficerNotActive);

            if (requestDto.Date.Date < DateTime.Today)
                throw new ArgumentException(ErrorMessages.Patrol.PastDate);

            var exists = await _patrolRepository.ExistsAsync(requestDto.OfficerId, requestDto.Date);
            if (exists)
                throw new InvalidOperationException(ErrorMessages.Patrol.AlreadyScheduled);

            var patrol = new Domain.Entity.Patrol
            {
                OfficerId = requestDto.OfficerId,
                Area = requestDto.Area,
                Date = requestDto.Date,
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
    }
}
