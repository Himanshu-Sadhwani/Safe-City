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

        /// <summary>
        /// Initializes a new instance of <see cref="FieldReportRepository"/>.
        /// </summary>
        public FieldReportRepository(SafeCityDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a field report by its unique identifier.
        /// </summary>
        public async Task<FieldReportEntity?> GetByIdAsync(int reportId)
        {
            return await _context.FieldReports.FindAsync(reportId);
        }

        /// <summary>
        /// Adds the field report to the database and saves changes.
        /// </summary>
        public async Task SaveAsync(FieldReportEntity entity)
        {
            await _context.FieldReports.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Returns <c>true</c> when a field report with the same patrol, notes (case-insensitive),
        /// and date (date part only) already exists.
        /// </summary>
        public async Task<bool> ExistsAsync(int patrolId, string notes, DateTime date)
        {
            return await _context.FieldReports.AnyAsync(fr =>
                fr.PatrolId == patrolId &&
                fr.Notes.ToLower() == notes.ToLower() &&
                fr.Date.Date == date.Date);
        }

        /// <summary>
        /// Saves changes to an existing tracked field report entity.
        /// </summary>
        public async Task UpdateAsync(FieldReportEntity entity)
        {
            _context.FieldReports.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
