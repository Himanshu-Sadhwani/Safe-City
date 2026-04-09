using SafeCity.Domain.Data;
using SafeCity.Domain.Enum;
using SafeCity.DTOs;
using SafeCity.Utility;
using Microsoft.EntityFrameworkCore;

namespace SafeCity.Repository.Compliance;

public class ComplianceRepository : IComplianceRepository
{
    private readonly SafeCityDbContext _context;

    public ComplianceRepository(SafeCityDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Saves a new compliance record to the database after the entity has been validated.
    /// </summary>
    /// <param name="request">The compliance request containing entity ID, type, result, and notes.</param>
    /// <returns>A response DTO with the details of the stored compliance record.</returns>
    /// <exception cref="Exception">Thrown when a database error occurs while saving.</exception>
    public async Task<CreateComplianceResponseDto> CreateComplianceAsync(CreateComplianceRequestDto request)
    {
        try
        {
            var compliance = request.ToComplianceEntity();

            await _context.ComplianceRecords.AddAsync(compliance);
            await _context.SaveChangesAsync();

            return new CreateComplianceResponseDto
            {
                ComplianceID = compliance.ComplianceID,
                EntityID = compliance.EntityID,
                Type = compliance.Type,
                Result = compliance.Result,
                Date = compliance.Date,
                Notes = compliance.Notes
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ErrorMessages.Compliance.SaveFailed, ex);
        }
    }

    /// <summary>
    /// Checks whether the referenced entity (Incident or Dispatch) exists in the database.
    /// </summary>
    /// <param name="entityId">The ID of the entity to validate.</param>
    /// <param name="type">The compliance type indicating which table to check.</param>
    /// <returns><c>true</c> if the entity exists; otherwise, <c>false</c>.</returns>
    public async Task<bool> IsValidEntityAsync(int entityId, ComplianceType type)
    {
        if (type == ComplianceType.Incident)
            return await _context.Incidents.AnyAsync(i => i.IncidentID == entityId);

        if (type == ComplianceType.Dispatch)
            return await _context.Dispatches.AnyAsync(d => d.DispatchID == entityId);

        return false;
    }
}
