using System.ComponentModel.DataAnnotations;

namespace SafeCity.DTOs.Patrol
{
    public class PatrolFilterDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Enter valid patrol Id")]
        public int? PatrolId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Enter valid Officer Id")]
        public int? OfficerId { get; set; }

        public DateTime? Date { get; set; }
    }
}
