using SafeCity.Domain.Data;
using SafeCity.Domain.Entity;
using SafeCity.Domain.Enum;
using SafeCity.DTOs;
using SafeCity.Utility;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace SafeCity.Repository.Compliance;

public class ComplianceRepository : IComplianceRepository
{
    private readonly SafeCityDbContext _context;
    private readonly IMapper _mapper;

    public ComplianceRepository(SafeCityDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Saves a new compliance record to the database after the entity has been validated.
    /// </summary>
    /// <param name="request">The compliance request containing entity ID, type, result, and notes.</param>
    /// <returns>A response DTO with the details of the stored compliance record.</returns>
    /// <exception cref="Exception">Thrown when a database error occurs while saving.</exception>
    public async Task CreateComplianceAsync(CreateComplianceRequestDto request)
    {
        try
        {
            var compliance = _mapper.Map<ComplianceRecord>(request);

            await _context.ComplianceRecords.AddAsync(compliance);
            await _context.SaveChangesAsync();
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

    /// <summary>
    /// Retrieves all compliance records with optional filters for type, result, and sort order.
    /// </summary>
    /// <param name="type">Optional filter by compliance type (Incident or Dispatch).</param>
    /// <param name="result">Optional filter by compliance result (Pass or Fail).</param>
    /// <param name="sort">Sort order: "asc" for ascending, defaults to descending.</param>
    /// <returns>A filtered and sorted list of compliance records.</returns>
    public async Task<List<GetComplianceResponseDto>> GetAllAsync(ComplianceType? type, ComplianceResult? result, string? sort)
    {
        try
        {
            var records = await _context.ComplianceRecords.ToListAsync();

            if (type.HasValue)
                records = records.Where(c => c.Type == type.Value).ToList();

            if (result.HasValue)
                records = records.Where(c => c.Result == result.Value).ToList();

            if (sort?.ToLower() == "asc")
                records = records.OrderBy(c => c.ComplianceID).ToList();
            else
                records = records.OrderByDescending(c => c.ComplianceID).ToList();

            return records.Select(c => new GetComplianceResponseDto
            {
                ComplianceID = c.ComplianceID,
                EntityID = c.EntityID,
                Type = c.Type.ToString(),
                Result = c.Result.ToString(),
                Date = c.Date,
                Notes = c.Notes
            }).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception(ErrorMessages.Compliance.FetchFailed, ex);
        }
    }

    /// <summary>
    /// Checks whether a compliance record already exists for the given entity.
    /// </summary>
    /// <param name="entityId">The ID of the entity to check.</param>
    /// <param name="type">The compliance type indicating which entity (Incident or Dispatch) to check against.</param>
    /// <returns><c>true</c> if a compliance record already exists for this entity; otherwise, <c>false</c>.</returns>
    public async Task<bool> AlreadyExistsAsync(int entityId, ComplianceType type)
    {
        return await _context.ComplianceRecords
            .AnyAsync(c => c.EntityID == entityId && c.Type == type);
    }
}
