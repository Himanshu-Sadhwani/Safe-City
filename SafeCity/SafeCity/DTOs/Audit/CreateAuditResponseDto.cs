using System;

namespace SafeCity.DTOs;

public class CreateAuditResponseDto
{
    public int AuditID { get; set; }
    public int OfficerID { get; set; }
    public string Scope { get; set; }
    public string Findings { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; }
}
