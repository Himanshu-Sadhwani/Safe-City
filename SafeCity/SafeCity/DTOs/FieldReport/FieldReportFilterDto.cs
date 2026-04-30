using SafeCity.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace SafeCity.DTOs.FieldReport
{
    public class FieldReportFilterDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Enter valid Report Id")]
        public int? ReportId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Enter valid patrol Id")]
        public int? PatrolId { get; set; }

        public DateTime? Date { get; set; }
        public FieldReportStatus? Status { get; set; }
    }
}
