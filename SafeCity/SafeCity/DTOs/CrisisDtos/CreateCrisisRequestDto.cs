using System;
using System.ComponentModel.DataAnnotations;
using SafeCity.Domain.Entity;
using SafeCity.Domain.Enum;
using SafeCity.Utility;

namespace SafeCity.DTOs.CrisisDtos;

public class CreateCrisisRequestDto
{
    [Required(ErrorMessage = ErrorMessages.Crisis.TypeRequired)]
    public CrisisType? Type { get; set; }
    [Required(ErrorMessage = ErrorMessages.Crisis.LocationRequired)]
    public string Location { get; set; } = string.Empty;
    [Required(ErrorMessage = ErrorMessages.Crisis.DateRequired)]
    public DateTime? Date { get; set; }
    [Required(ErrorMessage = ErrorMessages.Crisis.SeverityRequired)]
    public CrisisSeverity? Severity { get; set; }
    public CrisisStatus? Status { get; set; }
    public Crisis ToEntity()
    {
        return new Crisis
        {
            Type = Type!.Value,
            Location = Location,
            Date = Date!.Value,
            Severity = Severity!.Value,
            Status = Status ?? CrisisStatus.Pending
        };
    }

}
