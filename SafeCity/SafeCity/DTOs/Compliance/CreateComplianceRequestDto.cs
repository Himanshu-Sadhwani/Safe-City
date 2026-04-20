using SafeCity.Domain.Enum;

namespace SafeCity.DTOs;

public class CreateComplianceRequestDto
{
    public int EntityId {get; set; }
    public ComplianceType Type {get; set;}
    public ComplianceResult Result {get; set;}
    public string Notes {get; set;}
}
