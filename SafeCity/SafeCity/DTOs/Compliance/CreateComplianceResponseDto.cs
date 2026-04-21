using System;
using SafeCity.Domain.Enum;
using SafeCity.Domain.Entity;

namespace SafeCity.DTOs;

public class CreateComplianceResponseDto
{
    public int ComplianceID {get; set;}
    public int EntityID {get; set;}
    public ComplianceType Type {get; set;}
    public ComplianceResult Result {get; set;}
    public DateTime Date {get; set;}
    public string? Notes {get; set;}
}
