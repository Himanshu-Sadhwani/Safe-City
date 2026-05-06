using SafeCity.DTOs.FieldReport;

namespace SafeCity.Services.FieldReport
{
    /// <summary>
    /// Defines the contract for field report business logic operations.
    /// </summary>
    public interface IFieldReportService
    {
        Task<FieldReportResponseDto> CreateAsync(CreateFieldReportDto dto, int officerId);
        Task<FieldReportResponseDto> UpdateAsync(int reportId, UpdateFieldReportDto dto, int officerId);
        Task<IEnumerable<FieldReportResponseDto>> GetAllAsync(FieldReportFilterDto filter);
    }
}
