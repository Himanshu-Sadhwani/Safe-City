using Microsoft.EntityFrameworkCore;
using SafeCity.Domain.Data;
using SafeCity.DTOs.FieldReport;
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

        public async Task<IEnumerable<FieldReportEntity>> GetAllAsync(FieldReportFilterDto filter)
        {
            var query = _context.FieldReports.AsQueryable();

            if (filter.ReportId.HasValue)
                query = query.Where(f => f.ReportId == filter.ReportId.Value);

            if (filter.PatrolId.HasValue)
                query = query.Where(f => f.PatrolId == filter.PatrolId.Value);

            if (filter.Date.HasValue)
                query = query.Where(f => f.Date.Date == filter.Date.Value.Date);

            if (filter.Status.HasValue)
                query = query.Where(f => f.Status == filter.Status.Value);

            return await query.OrderByDescending(f => f.Date).ToListAsync();
        }
    }
}
