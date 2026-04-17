using System;
using System.Threading.Tasks;
using SafeCity.Domain.Entity;
using SafeCity.DTOs.CrisisDtos;
using SafeCity.Repository.CrisisRepo;
using SafeCity.Utility;

namespace SafeCity.Services.Crisis
{
    public class CrisisService : ICrisisService
    {
        private readonly ICrisisRepository _crisisRepository;
        /// <summary> 
        /// Initializes a new instance of the <see cref="CrisisService"/> class.
        /// </summary>
        /// <param name="crisisRepository"> Repository used to persist crisis data.</param>
        public CrisisService(ICrisisRepository crisisRepository)
        {
            _crisisRepository = crisisRepository;
        }
        /// <summary>
        /// Validates and declares a new crisis.
        /// </summary>
        /// <param name="request"> DTO containing crisis details such as location and date.</param>
        /// <returns> A <see cref="CrisisResponseDto"/> representing the created crisis.</returns>
        /// <exception cref="Exception"> Thrown when required fields such as location or date are missing. </exception>
        public async Task<CrisisResponseDto> DeclareCrisis(CreateCrisisRequestDto request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request), ErrorMessages.Crisis.RequestNull);

            if (string.IsNullOrWhiteSpace(request.Location))
                throw new Exception(ErrorMessages.Crisis.LocationRequired);

            if (request.Type == null)
                throw new Exception(ErrorMessages.Crisis.TypeRequired);

            if (!Enum.IsDefined(typeof(CrisisType), request.Type))
                throw new Exception(ErrorMessages.Crisis.InvalidType);

            if (request.Severity == null)
                throw new Exception(ErrorMessages.Crisis.SeverityRequired);

            if (!Enum.IsDefined(typeof(CrisisSeverity), request.Severity))
                throw new Exception(ErrorMessages.Crisis.InvalidSeverity);

            if (request.Date == null)
                throw new Exception(ErrorMessages.Crisis.DateRequired);

            if (request.Date.Value.Date < DateTime.UtcNow.Date)
                throw new Exception(ErrorMessages.Crisis.InvalidDate);

            if (request.Status != null && !Enum.IsDefined(typeof(CrisisStatus), request.Status))
                throw new Exception(ErrorMessages.Crisis.InvalidStatus);

            if (await _crisisRepository.IsDuplicateAsync(request))
                throw new Exception(ErrorMessages.Crisis.Duplicate);

            return await _crisisRepository.DeclareCrisis(request);
        }
    }
}