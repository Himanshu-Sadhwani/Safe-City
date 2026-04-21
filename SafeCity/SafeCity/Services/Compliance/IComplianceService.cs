using System;
using SafeCity.DTOs;

namespace SafeCity.Services.Compliance;

public interface IComplianceService
{
    public Task<CreateComplianceResponseDto> CreateComplianceAsync(CreateComplianceRequestDto request);
}