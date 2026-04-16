using SafeCity.Domain.Enum;

namespace SafeCity.DTOs.Case
{
    public class CaseCreation
    {
        public int IncidentID { get; set; }
        public int AssignedOfficerID { get; set; }
        public string Description { get; set; }
        public CaseStatusCheck Status { get; set; } = CaseStatusCheck.Open;
    }
}
