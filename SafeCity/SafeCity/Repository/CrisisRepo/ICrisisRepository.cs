using System.Threading.Tasks;
using SafeCity.DTOs.CrisisDtos;

namespace SafeCity.Repository.CrisisRepo
{
    public interface ICrisisRepository
    {
        public Task<CrisisResponseDto> DeclareCrisis(CreateCrisisRequestDto request);
        Task<bool> IsDuplicateAsync(CreateCrisisRequestDto request);
    }
}