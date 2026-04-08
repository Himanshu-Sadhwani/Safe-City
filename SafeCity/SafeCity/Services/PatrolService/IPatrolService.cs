using SafeCity.DTOs.Patrol;

namespace SafeCity.Services.PatrolService
{
    public interface IPatrolService
    {
        Task<List<AvailableOfficerDto>> GetAvailableOfficersAsync(DateTime date);
        Task<CreatePatrolResponseDto> CreatePatrolAsync(CreatePatrolRequestDto requestDto);
    }
}
