using System.Threading.Tasks;
using SafeCity.DTOs.CrisisDtos;
 
namespace SafeCity.Services.Crisis
{
    public interface ICrisisService
    {
        public Task<CrisisResponseDto> DeclareCrisis(CreateCrisisRequestDto request);
    }
}