using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SafeCity.Utility;

namespace SafeCity.DTOs.Patrol
{
    /// <summary>
    /// Request DTO for creating a new patrol assignment.
    /// </summary>
    public class CreatePatrolRequestDto
    {
        [Required(ErrorMessage = "Officer Id is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Enter valid officer id")]
        [JsonConverter(typeof(PatrolValidationHelper))]        public int? OfficerId { get; set; }

        [Required(ErrorMessage = "Area is required")]
        [MaxLength(100, ErrorMessage = "Area cannot exceed 100 characters")]
        public string? Area { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime? Date { get; set; }
    }
}
