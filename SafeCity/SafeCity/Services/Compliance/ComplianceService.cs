using System;
using SafeCity.Domain.Enum;
using SafeCity.DTOs;
using SafeCity.Repository.Compliance;
using SafeCity.Utility;

namespace SafeCity.Services.Compliance;

public class ComplianceService : IComplianceService
{
    private readonly IComplianceRepository _repo;

    public ComplianceService(IComplianceRepository repo)
    {
        _repo = repo;
    }

    public async Task<CreateComplianceResponseDto> CreateComplianceAsync(CreateComplianceRequestDto request)
    {
        if(request == null)
            throw new ArgumentNullException(nameof(request), ErrorMessages.Compliance.RequestNull);

        var errors = new List<string>();

        if(!Enum.IsDefined(typeof(ComplianceType), request.Type))
            errors.Add(ErrorMessages.Compliance.InvalidType);
        else if(request.EntityId <= 0)
            errors.Add(ErrorMessages.Compliance.InvalidEntityID);
        else if(!await _repo.IsValidEntityAsync(request.EntityId, request.Type))
            errors.Add(ErrorMessages.Compliance.EntityNotFound);
        
        if(!Enum.IsDefined(typeof(ComplianceResult), request.Result))
            errors.Add(ErrorMessages.Compliance.InvalidResult);

        if(errors.Any())
            throw new ArgumentException(string.Join(" | ", errors));

        return await _repo.CreateComplianceAsync(request);
    }
}