using SafeCity.DTOs.Response;
 
namespace SafeCity.Services.Response
{
    public interface IResponseService
    {
        Task<AssignResponseTeamResponseDto> AssignResponseTeamAsync(AssignResponseTeamRequestDto dto);
        Task<List<GetCrisisResponseDto>> GetCrisisWithResponseAsync(GetCrisisResponseRequestDto request);
    }
}