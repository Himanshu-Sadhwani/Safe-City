using Microsoft.EntityFrameworkCore;
using SafeCity.Domain.Data;
using SafeCity.DTOs.Response;
using ResponseEntity = SafeCity.Domain.Entity.Response;

namespace SafeCity.Repository.Response
{
    public class ResponseRepository : IResponseRepository
    {
        private readonly SafeCityDbContext _context;

        public ResponseRepository(SafeCityDbContext context)
        {
            _context = context;
        }

        // Check if Crisis exists
        public async Task<bool> CrisisExistsAsync(int crisisId)
        {
            return await _context.Crises
                                 .AsNoTracking()
                                 .AnyAsync(c => c.CrisisID == crisisId);
        }

        // Check if Team exists
        public async Task<bool> TeamExistsAsync(int teamId)
        {
            return await _context.Teams
                                 .AsNoTracking()
                                 .AnyAsync(t => t.TeamID == teamId);
        }

        // Assign Response Team (Add Response)
        public async Task<ResponseEntity> AddAsync(ResponseEntity response)
        {
            await _context.Responses.AddAsync(response);
            await _context.SaveChangesAsync();
            return response;
        }

        public async Task<bool> IsDuplicateAsync(int crisisId, int teamId)
        {
            return await _context.Responses.AnyAsync(r => r.CrisisID == crisisId && r.TeamID == teamId);
        }

        public async Task<List<GetCrisisResponseDto>> GetCrisisWithResponseAsync(GetCrisisResponseRequestDto request)
        {
            var query = from c in _context.Crises
                        join r in _context.Responses
                        on c.CrisisID equals r.CrisisID into responseGroup
                        from r in responseGroup.DefaultIfEmpty()
                        select new GetCrisisResponseDto
                        {
                            CrisisId = c.CrisisID,
                            Location = c.Location,
                            Severity = c.Severity.ToString(),
                            Status = c.Status.ToString(),

                            IsResponseAssigned = r != null,
                            TeamId = r != null ? r.TeamID : null,
                            Actions = r != null ? r.Actions : null
                        };

            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                query = query.Where(x => x.Status == request.Status);
            }

            if (request.TeamId.HasValue)
            {
                query = query.Where(x => x.TeamId == request.TeamId);
            }

            return await query.AsNoTracking().ToListAsync();
        }
    }
}