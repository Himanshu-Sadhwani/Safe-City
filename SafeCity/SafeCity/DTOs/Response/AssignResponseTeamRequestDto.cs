namespace SafeCity.DTOs.Response
{
    public class AssignResponseTeamRequestDto
    {
        public int CrisisId { get; set; }
        public int TeamId { get; set; }
        public string Actions { get; set; } = string.Empty;
    }
}