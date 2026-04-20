namespace SafeCity.DTOs.Response
{
    public class AssignResponseTeamResponseDto
    {
        public int ResponseId { get; set; }
        public int CrisisId { get; set; }
        public int TeamId { get; set; }
        public string Actions { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}