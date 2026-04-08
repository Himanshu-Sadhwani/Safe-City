namespace SafeCity.DTOs.Patrol
{
    public class CreatePatrolResponseDto
    {
        public int PatrolId { get; set; }
        public int OfficerId { get; set; }
        public string Area { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
