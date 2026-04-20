using Microsoft.EntityFrameworkCore;
using SafeCity.Domain.Data;
using FieldReportEntity = SafeCity.Domain.Entity.FieldReport;

namespace SafeCity.Repository.FieldReport
{
    /// <summary>
    /// EF Core implementation of <see cref="IFieldReportRepository"/> using <see cref="SafeCityDbContext"/>.
    /// </summary>
    public class FieldReportRepository : IFieldReportRepository
    {
        private readonly SafeCityDbContext _context;

        public FieldReportRepository(SafeCityDbContext context)
        {
            _context = context;
        }

        public async Task<FieldReportEntity?> GetByIdAsync(int reportId)
        {
            return await _context.FieldReports.FindAsync(reportId);
        }

        public async Task SaveAsync(FieldReportEntity entity)
        {
            await _context.FieldReports.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int patrolId, string notes, DateTime date)
        {
            return await _context.FieldReports.AnyAsync(fr =>
                fr.PatrolId == patrolId &&
                fr.Notes.ToLower() == notes.ToLower() &&
                fr.Date.Date == date.Date);
        }

        public async Task UpdateAsync(FieldReportEntity entity)
        {
            _context.FieldReports.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
