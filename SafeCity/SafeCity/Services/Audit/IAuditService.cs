using System;
using SafeCity.DTOs;

namespace SafeCity.Services.Audit;

public interface IAuditService
{
    Task<CreateAuditResponseDto> CreateAuditAsync(CreateAuditRequestDto request);
}
