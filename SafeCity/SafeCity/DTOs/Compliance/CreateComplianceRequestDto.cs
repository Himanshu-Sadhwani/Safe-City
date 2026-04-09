using System;
using SafeCity.Domain.Enum;
using SafeCity.Domain.Entity;

namespace SafeCity.DTOs;

public class CreateComplianceRequestDto
{
    public int EntityId {get; set; }
    public ComplianceType Type {get; set;}
    public ComplianceResult Result {get; set;}
    public string Notes {get; set;}

    public ComplianceRecord ToComplianceEntity()
    {
        return new ComplianceRecord
        {
            EntityID = EntityId,
            Type = Type,
            Result = Result,
            Date = DateTime.UtcNow,
            Notes = Notes
        };
    }
}
