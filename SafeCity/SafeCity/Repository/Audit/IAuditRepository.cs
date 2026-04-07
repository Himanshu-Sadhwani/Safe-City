using System;
using SafeCity.DTOs;

namespace SafeCity.Repository.Audit;

public interface IAuditRepository
{
    Task<CreateAuditResponseDto> CreateAuditAsync(CreateAuditRequestDto request);
}
