using System;
using SafeCity.Domain.Enum;

namespace SafeCity.DTOs;

public class DispatchUpdateByStatusRequestDto
{ 
    public DispatchStatusOption Status { get; set; }
}
