using SafeCity.Domain.Entity;
using SafeCity.Domain.Enum;

namespace SafeCity.DTOs;

public class CreateAuditRequestDto
{
    public int OfficerID {get; set;}
    public AuditScope Scope {get; set;}
    public string Findings {get; set;}
    public AuditStatus Status {get; set;}

    public Audit ToAuditEntity()
    {
        return new Audit
        {
            OfficerID = OfficerID,
            Scope = Scope,
            Findings = Findings,
            Date = DateTime.UtcNow,
            Status = Status
        };
    }
}
