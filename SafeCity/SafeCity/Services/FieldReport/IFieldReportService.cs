using SafeCity.DTOs.FieldReport;

namespace SafeCity.Services.FieldReport
{
    /// <summary>
    /// Defines the contract for field report business logic operations.
    /// </summary>
    public interface IFieldReportService
    {
        /// <summary>
        /// Validates and persists a new field report, then publishes a <c>FieldReportCreated</c> domain event.
        /// Throws <see cref="SafeCity.Utility.ForbiddenException"/> if the officer does not own the patrol.
        /// </summary>
        /// <param name="dto">The creation request containing patrol ID, notes, and date.</param>
        /// <param name="officerId">The ID of the authenticated officer extracted from the JWT.</param>
        Task<FieldReportResponseDto> CreateAsync(CreateFieldReportDto dto, int officerId);

        /// <summary>
        /// Partially updates the notes and/or status of an existing field report.
        /// Throws <see cref="SafeCity.Utility.ForbiddenException"/> if the officer does not own the report.
        /// </summary>
        /// <param name="reportId">The ID of the report to update.</param>
        /// <param name="dto">Fields to apply; null properties are left unchanged.</param>
        /// <param name="officerId">The ID of the authenticated officer extracted from the JWT.</param>
        Task<FieldReportResponseDto> UpdateAsync(int reportId, UpdateFieldReportDto dto, int officerId);
    }
}
