using System;
using SafeCity.DTOs;
using SafeCity.Repository.Audit;
using SafeCity.Utility;
using SafeCity.Domain.Enum;

namespace SafeCity.Services.Audit;

public class AuditService : IAuditService
{
    private readonly IAuditRepository _repo;

    public AuditService(IAuditRepository repo)
    {
        _repo = repo;
    }

    /// <summary>
    /// Validates the audit request and delegates the creation of a new audit record to the repository.
    /// </summary>
    /// <param name="request">The audit request containing officer ID, scope, findings, and status.</param>
    /// <returns>A response DTO containing the details of the newly created audit record.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the request object is null.</exception>
    /// <exception cref="ArgumentException">Thrown when validation fails, including invalid officer ID, missing findings, or invalid scope/status values.</exception>
    public async Task<CreateAuditResponseDto> CreateAuditAsync(CreateAuditRequestDto request)
    {
        if(request == null)
            throw new ArgumentNullException(nameof(request), ErrorMessages.Audit.RequestNull);

        var errors = new List<string>();

        if(request.OfficerID == 0)
            errors.Add(ErrorMessages.Audit.OfficerIDRequired);
        else if(request.OfficerID < 0)
            errors.Add(ErrorMessages.Audit.InvalidOfficerID);
        else if (!await _repo.IsValidOfficerAsync(request.OfficerID))
            errors.Add(ErrorMessages.Audit.OfficerNotFound);
        
        if(string.IsNullOrWhiteSpace(request.Findings))
            errors.Add(ErrorMessages.Audit.FindingsRequired);

        if (!Enum.IsDefined(typeof(AuditScope), request.Scope))
            errors.Add(ErrorMessages.Audit.InvalidScope);

        if (!Enum.IsDefined(typeof(AuditStatus), request.Status))
            errors.Add(ErrorMessages.Audit.InvalidStatus);

        if(errors.Any())
            throw new ArgumentException(string.Join(" | ", errors));

        return await _repo.CreateAuditAsync(request);
    }
}
