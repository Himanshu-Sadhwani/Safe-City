using FieldReportEntity = SafeCity.Domain.Entity.FieldReport;

namespace SafeCity.Repository.FieldReport
{
    /// <summary>
    /// Defines the contract for field report data access operations.
    /// </summary>
    public interface IFieldReportRepository
    {
        Task<FieldReportEntity?> GetByIdAsync(int reportId);
        Task SaveAsync(FieldReportEntity entity);
        Task<bool> ExistsAsync(int patrolId, string notes, DateTime date);
        Task UpdateAsync(FieldReportEntity entity);
    }
}
