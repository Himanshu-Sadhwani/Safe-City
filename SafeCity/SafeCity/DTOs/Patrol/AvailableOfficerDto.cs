namespace SafeCity.DTOs.Patrol
{
    public class AvailableOfficerDto
    {
        public int OfficerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
}
