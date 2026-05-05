using System;
using SafeCity.Domain.Enum;
using SafeCity.DTOs;

namespace SafeCity.Repository.Compliance;

public interface IComplianceRepository
{
    public Task<CreateComplianceResponseDto> CreateComplianceAsync(CreateComplianceRequestDto request);
    public Task<bool> IsValidEntityAsync(int entityId, ComplianceType type);
}
