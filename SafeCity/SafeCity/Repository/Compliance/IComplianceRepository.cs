using System;
using SafeCity.Domain.Enum;
using SafeCity.DTOs;

namespace SafeCity.Repository.Compliance;

public interface IComplianceRepository
{
    public Task CreateComplianceAsync(CreateComplianceRequestDto request);
    public Task<bool> IsValidEntityAsync(int entityId, ComplianceType type);
    public Task<bool> AlreadyExistsAsync(int entityId, ComplianceType type);
    public Task<List<GetComplianceResponseDto>> GetAllAsync(ComplianceType? type, ComplianceResult? result, string? sort);
}
