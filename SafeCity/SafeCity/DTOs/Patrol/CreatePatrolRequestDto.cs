using System.ComponentModel.DataAnnotations;

namespace SafeCity.DTOs.Patrol
{
    public class CreatePatrolRequestDto
    {
        [Required(ErrorMessage = "OfficerId is required")]
        public int OfficerId { get; set; }

        [Required(ErrorMessage = "Area is required")]
        [MaxLength(100, ErrorMessage = "Area cannot exceed 100 characters")]
        public string Area { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }
    }
}
