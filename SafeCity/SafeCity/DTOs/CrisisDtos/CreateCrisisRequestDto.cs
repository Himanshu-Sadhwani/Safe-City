using System;
using SafeCity.Domain.Entity;
using SafeCity.Domain.Enum;

namespace SafeCity.DTOs.CrisisDtos;

public class CreateCrisisRequestDto
{
    public CrisisType Type { get; set; }
    public string Location { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public CrisisSeverity Severity { get; set; }
    public CrisisStatus Status { get; set; }
    public Crisis ToEntity()
    {
        return new Crisis
        {
            Type = Type,
            Location = Location,
            Date = Date,
            Severity = Severity,
            Status = Status
        };
    }

}
