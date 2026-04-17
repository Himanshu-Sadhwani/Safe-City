using System.Threading.Tasks;
using SafeCity.DTOs.CrisisDtos;
using SafeCity.Domain.Entity;
using SafeCity.Domain.Data;
using Microsoft.EntityFrameworkCore;
using SafeCity.Utility;

namespace SafeCity.Repository.CrisisRepo
{
    public class CrisisRepository : ICrisisRepository
    {
        private readonly SafeCityDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="CrisisRepository"/> class.
        /// </summary>

        public CrisisRepository(SafeCityDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Declares a new crisis by persisting the provided request details to the database.
        /// </summary>
        /// <param name="request"> DTO containing all information required to create a new crisis entry.</param>
        /// <returns> A <see cref="CrisisResponseDto"/> representing the newly created crisis.</returns>
        /// <exception cref="ArgumentNullException"> Thrown when the request object is null. </exception>
        /// <exception cref="Exception">Thrown when a database save operation fails.</exception>

        public async Task<CrisisResponseDto> DeclareCrisis(CreateCrisisRequestDto request)
        {
            try
            {
                var crisis = request.ToEntity();
                await _context.Set<Crisis>().AddAsync(crisis);
                await _context.SaveChangesAsync();

                return CrisisResponseDto.FromEntity(crisis);
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorMessages.Crisis.DeclarationFailed, ex);
            }
        }

        public async Task<bool> IsDuplicateAsync(CreateCrisisRequestDto request)
        {
            return await _context.Set<Crisis>().AnyAsync(c => c.Type == request.Type && c.Location == request.Location && c.Date.Date == request.Date!.Value.Date);
        }
    }
}