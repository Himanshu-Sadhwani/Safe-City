using System;
using SafeCity.Domain.Enum;

namespace SafeCity.DTOs.Resource;

public class GetResourceResponseDto
{
    public int ResourceID { get; set; }
    public ResourceTypeOption Type { get; set; }
    public ResourceAvailabilityOption Availability { get; set; }
    public string Location { get; set; }
    public string UnitName { get; set; }
}
