using SafeCity.Domain.Entity;
using SafeCity.DTOs.Response;
using ResponseEntity = SafeCity.Domain.Entity.Response;

namespace SafeCity.Repository.Response
{
    public interface IResponseRepository
    {
        Task<ResponseEntity> AddAsync(ResponseEntity response);
        Task<bool> CrisisExistsAsync(int crisisId);
        Task<bool> TeamExistsAsync(int teamId);
        Task<bool> IsDuplicateAsync(int crisisId, int teamId);
        Task<List<GetCrisisResponseDto>> GetCrisisWithResponseAsync(GetCrisisResponseRequestDto request);
    }
}