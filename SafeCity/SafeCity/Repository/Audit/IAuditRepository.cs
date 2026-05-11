using System;
using SafeCity.DTOs;
using SafeCity.Domain.Enum;

namespace SafeCity.Repository.Audit;

public interface IAuditRepository
{
    Task<CreateAuditResponseDto> CreateAuditAsync(CreateAuditRequestDto request);
    Task<bool> IsValidOfficerAsync(int officerId);
    Task<List<CreateAuditResponseDto>> GetAllAsync(AuditScope? scope, AuditStatus? status, int? officerId, string? sort);
}
