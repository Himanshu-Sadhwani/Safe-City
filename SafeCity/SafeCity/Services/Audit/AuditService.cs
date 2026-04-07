using System;
using SafeCity.DTOs;
using SafeCity.Repository.Audit;
using SafeCity.Utility;
// using SafeCity.Repository;
// using SafeCity.Utility;

namespace SafeCity.Services.Audit;

public class AuditService : IAuditService
{
    private readonly IAuditRepository _repo;

    public AuditService(IAuditRepository repo)
    {
        _repo = repo;
    }
    public async Task<CreateAuditResponseDto> CreateAuditAsync(CreateAuditRequestDto request)
    {
        if(request == null)
            throw new ArgumentNullException(nameof(request), ErrorMessages.Audit.RequestNull);

        var errors = new List<string>();

        if(request.OfficerID <= 0)
            errors.Add(ErrorMessages.Audit.InvalidOfficerID);
        
        if(string.IsNullOrWhiteSpace(request.Findings))
            errors.Add(ErrorMessages.Audit.FindingsRequired);

        if(errors.Any())
            throw new ArgumentException(string.Join(" | ", errors));

        return await _repo.CreateAuditAsync(request);
    }
}
