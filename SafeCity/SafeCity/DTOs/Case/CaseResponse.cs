namespace SafeCity.DTOs.Case;

public class CaseResponse
{
    public int CaseID { get; set; }
    public int IncidentID { get; set; }
    public int AssignedOfficerID { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public DateTime ResolutionDate { get; set; }
}