using System;
using SafeCity.Domain.Enum;

namespace SafeCity.DTOs.Dispatch;

public class GetResponseDto
{   
    public int DispatchID { get; set; }
    public int IncidentId { get; set; }
    public int DispatcherId { get; set; }
    public string DispatcherName { get; set; }
    public int ResourceId { get; set; }
    public DateTime Date { get; set; }
    public DispatchStatusOption Status { get; set; }

}
