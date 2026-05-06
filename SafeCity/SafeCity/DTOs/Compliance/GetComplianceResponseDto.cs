using SafeCity.Domain.Enum;

namespace SafeCity.DTOs;

public class GetComplianceResponseDto
{
    public int ComplianceID { get; set; }
    public int EntityID { get; set; }
    public string Type { get; set; }
    public string Result { get; set; }
    public DateTime Date { get; set; }
    public string Notes { get; set; }
}
