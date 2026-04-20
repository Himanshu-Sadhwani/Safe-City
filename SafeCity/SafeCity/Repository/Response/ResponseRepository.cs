using Microsoft.EntityFrameworkCore;
using SafeCity.Domain.Data;
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
    }
}