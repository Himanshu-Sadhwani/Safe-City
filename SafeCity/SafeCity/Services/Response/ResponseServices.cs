using SafeCity.DTOs.Response;
using SafeCity.Domain.Enum;
using SafeCity.Repository.Response;
using ResponseEntity = SafeCity.Domain.Entity.Response;
using SafeCity.Domain.Entity;
using SafeCity.Utility;

namespace SafeCity.Services.Response
{
    public class ResponseService : IResponseService
    {
        private readonly IResponseRepository _repository;

        public ResponseService(IResponseRepository repository)
        {
            _repository = repository;
        }

        public async Task<AssignResponseTeamResponseDto> AssignResponseTeamAsync(AssignResponseTeamRequestDto dto)
        {
            //Duplicate
            if (await _repository.IsDuplicateAsync(dto.CrisisId, dto.TeamId))
                throw new InvalidOperationException(ErrorMessages.Response.Duplicate);

            // Validate Crisis
            if (!await _repository.CrisisExistsAsync(dto.CrisisId))
                throw new KeyNotFoundException(ErrorMessages.Response.CrisisNotFound);

            // Validate Team
            if (!await _repository.TeamExistsAsync(dto.TeamId))
                throw new KeyNotFoundException(ErrorMessages.Response.TeamNotFound);

            // Create Response Entity
            var response = new ResponseEntity
            {
                CrisisID = dto.CrisisId,
                TeamID = dto.TeamId,
                Actions = dto.Actions,
                Date = DateTime.UtcNow,
                Status = ResponseStatus.Active
            };

            var result = await _repository.AddAsync(response);

            // Map to DTO
            return new AssignResponseTeamResponseDto
            {
                ResponseId = result.ResponseID,
                CrisisId = result.CrisisID,
                TeamId = result.TeamID,
                Actions = result.Actions,
                Status = result.Status.ToString(),
                CreatedAt = result.Date
            };
        }

        public async Task<List<GetCrisisResponseDto>> GetCrisisWithResponseAsync(GetCrisisResponseRequestDto request)
        {
            if (request.TeamId < 0)
            {
                throw new ArgumentException(
                    ErrorMessages.Response.InvalidTeamId);
            }

            return await _repository
                .GetCrisisWithResponseAsync(request);
        }
    }
}