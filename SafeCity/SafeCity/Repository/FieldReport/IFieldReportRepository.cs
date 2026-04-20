using FieldReportEntity = SafeCity.Domain.Entity.FieldReport;

namespace SafeCity.Repository.FieldReport
{
    /// <summary>
    /// Defines the contract for field report data access operations.
    /// </summary>
    public interface IFieldReportRepository
    {
        /// <summary>
        /// Retrieves a field report by its unique identifier.
        /// </summary>
        /// <param name="reportId">The primary key of the field report.</param>
        /// <returns>The matching <see cref="FieldReportEntity"/>, or <c>null</c> if not found.</returns>
        Task<FieldReportEntity?> GetByIdAsync(int reportId);

        /// <summary>
        /// Persists a new field report to the database.
        /// </summary>
        /// <param name="entity">The <see cref="FieldReportEntity"/> entity to save.</param>
        Task SaveAsync(FieldReportEntity entity);

        /// <summary>
        /// Checks whether an identical field report already exists for the given patrol, notes, and date.
        /// </summary>
        /// <param name="patrolId">The patrol identifier.</param>
        /// <param name="notes">The report notes (compared case-insensitively).</param>
        /// <param name="date">The report date (compared by date part only).</param>
        /// <returns><c>true</c> if a duplicate exists; otherwise <c>false</c>.</returns>
        Task<bool> ExistsAsync(int patrolId, string notes, DateTime date);

        /// <summary>
        /// Persists changes to an existing field report.
        /// </summary>
        /// <param name="entity">The tracked <see cref="FieldReportEntity"/> with updated values.</param>
        Task UpdateAsync(FieldReportEntity entity);
    }
}
