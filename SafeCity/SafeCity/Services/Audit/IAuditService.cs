using System;
using SafeCity.Domain.Enum;
using SafeCity.DTOs;

namespace SafeCity.Services.Audit;

public interface IAuditService
{
    Task<CreateAuditResponseDto> CreateAuditAsync(CreateAuditRequestDto request);
    Task<List<CreateAuditResponseDto>> GetAllAsync(AuditScope? scope, AuditStatus? status, int? officerId, string? sort);
}
