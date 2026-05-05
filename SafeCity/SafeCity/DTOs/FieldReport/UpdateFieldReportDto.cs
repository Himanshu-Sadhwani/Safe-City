using System.ComponentModel.DataAnnotations;
using SafeCity.Domain.Enum;
using SafeCity.Utility;

namespace SafeCity.DTOs.FieldReport
{
    /// <summary>
    /// Data transfer object for partially updating an existing field report.
    /// All fields are optional; only non-null fields are applied.
    /// </summary>
    public class UpdateFieldReportDto
    {
        [MinLength(50, ErrorMessage = ErrorMessages.FieldReport.NotesTooShort)]
        [MaxLength(100, ErrorMessage = ErrorMessages.FieldReport.NotesTooLong)]
        public string? Notes { get; set; }

        [EnumDataType(typeof(FieldReportStatus), ErrorMessage = ErrorMessages.FieldReport.NosuchStatus)]
        public FieldReportStatus? Status { get; set; }
    }
}
