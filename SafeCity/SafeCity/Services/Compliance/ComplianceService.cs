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

    /// <summary>
    /// Validates the compliance request and delegates the creation of a new compliance record to the repository.
    /// </summary>
    /// <param name="request">The compliance request containing entity ID, type, result, and notes.</param>
    /// <exception cref="ArgumentNullException">Thrown when the request object is null.</exception>
    /// <exception cref="ArgumentException">Thrown when validation fails, including invalid entity ID, duplicate record, or invalid type/result values.</exception>
    public async Task CreateComplianceAsync(CreateComplianceRequestDto request)
    {
        if(request == null)
            throw new ArgumentNullException(nameof(request), ErrorMessages.Compliance.RequestNull);

        var errors = new List<string>();

        if(!Enum.IsDefined(typeof(ComplianceType), request.Type))
            errors.Add(ErrorMessages.Compliance.InvalidType);

        if(request.EntityId == 0)
            errors.Add(ErrorMessages.Compliance.EntityIdRequired);
        else if(request.EntityId < 0)
            errors.Add(ErrorMessages.Compliance.InvalidEntityID);
        else if(!await _repo.IsValidEntityAsync(request.EntityId, request.Type))
            errors.Add(ErrorMessages.Compliance.EntityNotFound);
        else if(await _repo.AlreadyExistsAsync(request.EntityId, request.Type))
            errors.Add(ErrorMessages.Compliance.DuplicateRecord);


        if(!Enum.IsDefined(typeof(ComplianceResult), request.Result))
            errors.Add(ErrorMessages.Compliance.InvalidResult);

        if(string.IsNullOrWhiteSpace(request.Notes))
            errors.Add(ErrorMessages.Compliance.NotesRequired);

        if(errors.Any())
            throw new ArgumentException(string.Join(" | ", errors));

        await _repo.CreateComplianceAsync(request);
    }
}