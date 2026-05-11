namespace SafeCity.DTOs.Response
{
    public class GetCrisisResponseDto
    {
        public int CrisisId { get; set; }
        public string Location { get; set; }
        public string Severity { get; set; }
        public string Status { get; set; }
 
        public bool IsResponseAssigned { get; set; }
        public int? TeamId { get; set; }
        public string Actions { get; set; }
    }
}