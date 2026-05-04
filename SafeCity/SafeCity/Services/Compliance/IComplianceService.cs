using System;
using SafeCity.DTOs;
using SafeCity.Domain.Enum;

namespace SafeCity.Services.Compliance;

public interface IComplianceService
{
    public Task CreateComplianceAsync(CreateComplianceRequestDto request);
    public Task<List<GetComplianceResponseDto>> GetAllAsync(ComplianceType? type, ComplianceResult? result, string? sort);
}