using System.ComponentModel.DataAnnotations;
using SafeCity.Utility;

namespace SafeCity.DTOs.FieldReport
{
    /// <summary>
    /// Data transfer object for creating a new field report.
    /// </summary>
    public class CreateFieldReportDto
    {
        [Required(ErrorMessage = ErrorMessages.FieldReport.PatrolIdRequired)]
        [Range(1, int.MaxValue, ErrorMessage = ErrorMessages.FieldReport.InvalidPatrolId)]
        public int PatrolId { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldReport.NotesRequired)]
        [MinLength(50, ErrorMessage = ErrorMessages.FieldReport.NotesTooShort)]
        [MaxLength(100, ErrorMessage = ErrorMessages.FieldReport.NotesTooLong)]
        public string Notes { get; set; } = string.Empty;

        [Required(ErrorMessage = ErrorMessages.FieldReport.DateRequired)]
        [FutureOrTodayDate(
            ErrorMessage = ErrorMessages.FieldReport.PastDate,
            FutureDateErrorMessage = ErrorMessages.FieldReport.FutureDate)]
        public DateTime? Date { get; set; }
    }
}
