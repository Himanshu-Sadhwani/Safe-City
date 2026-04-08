using System;
using SafeCity.Domain.Enum;

namespace SafeCity.DTOs;

public class DispatchResponseDto
{
    public int DispatchId { get; set; }
    public int IncidentId { get; set; }
    public int ResourceId { get; set; }
    public string UnitName { get; set; }
    public DispatchStatusOption Status { get; set; }
    public DateTime DispatchDateTime { get; set; }
}
