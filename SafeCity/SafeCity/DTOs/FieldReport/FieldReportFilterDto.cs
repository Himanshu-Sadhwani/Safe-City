using SafeCity.Domain.Enum;

namespace SafeCity.DTOs.FieldReport
{
    public class FieldReportFilterDto
    {
        public int? ReportId { get; set; }
        public int? PatrolId { get; set; }
        public DateTime? Date { get; set; }
        public FieldReportStatus? Status { get; set; }
    }
}
