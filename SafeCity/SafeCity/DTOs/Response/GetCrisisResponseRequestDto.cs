using SafeCity.Domain.Entity;

namespace SafeCity.DTOs.Response
{
    public class GetCrisisResponseRequestDto
    {
        public CrisisStatus? Status { get; set; }
        public CrisisSeverity? Severity { get; set; }
        public int? TeamId { get; set; }
        public int? CrisisId { get; set; }
        public string? Location { get; set; }
    }
}