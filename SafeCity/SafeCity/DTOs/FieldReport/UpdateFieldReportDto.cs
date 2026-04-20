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
        /// <summary>
        /// Gets or sets the updated notes. Must be between 50 and 100 characters if provided.
        /// </summary>
        [MinLength(50, ErrorMessage = ErrorMessages.FieldReport.NotesTooShort)]
        [MaxLength(100, ErrorMessage = ErrorMessages.FieldReport.NotesTooLong)]
        public string? Notes { get; set; }

        /// <summary>
        /// Gets or sets the updated report status. Must be a valid <see cref="FieldReportStatus"/> value if provided.
        /// </summary>
        public FieldReportStatus? Status { get; set; }
    }
}
