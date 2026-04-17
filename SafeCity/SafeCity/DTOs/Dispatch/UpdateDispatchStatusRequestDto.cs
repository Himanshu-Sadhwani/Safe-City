using System;
using SafeCity.Domain.Enum;

namespace SafeCity.DTOs;

public class UpdateDispatchStatusRequestDto
{ 
    public int DispatchId { get; set; }
    public DispatchStatusOption Status { get; set; }
}
