namespace SafeCity.DTOs.Patrol
{
    public class PatrolResponseDto
    {
        public int PatrolId { get; set; }
        public int OfficerId { get; set; }
        public string Area { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
    }
}
