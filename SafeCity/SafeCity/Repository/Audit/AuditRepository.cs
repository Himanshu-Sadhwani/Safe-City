using System;
using SafeCity.Utility;
using SafeCity.DTOs;
using SafeCity.Domain.Data;
using SafeCity.Domain.Enum;
using Microsoft.EntityFrameworkCore;

namespace SafeCity.Repository.Audit;

public class AuditRepository : IAuditRepository
{
    private readonly SafeCityDbContext _context;

    public AuditRepository(SafeCityDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Handles the database logic for saving a new audit record submitted by a Compliance Officer.
    /// </summary>
    /// <param name="request">The audit request containing officer ID, scope, findings, and status.</param>
    /// <returns>A response DTO containing the details of the newly created audit record.</returns>
    /// <exception cref="Exception">Thrown when a database error occurs while saving the audit.</exception>
    public async Task<CreateAuditResponseDto> CreateAuditAsync(CreateAuditRequestDto request)
    {
        try
        {
            var audit = request.ToAuditEntity();

            await _context.Audits.AddAsync(audit);
            await _context.SaveChangesAsync();

            return new CreateAuditResponseDto
            {
                AuditID = audit.AuditID,
                OfficerID = audit.OfficerID,
                Scope = audit.Scope.ToString(),
                Findings = audit.Findings,
                Date = audit.Date,
                Status = audit.Status.ToString()
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ErrorMessages.Audit.SaveFailed, ex);
        }
    }

    /// <summary>
    /// Retrieves all audit records with optional filters for scope, status, officer, and sort order.
    /// </summary>
    /// <param name="scope">Optional filter by audit scope.</param>
    /// <param name="status">Optional filter by audit status.</param>
    /// <param name="officerId">Optional filter by officer ID.</param>
    /// <param name="sort">Sort order: "asc" for ascending, defaults to descending.</param>
    /// <returns>A filtered and sorted list of audit records.</returns>
    public async Task<List<CreateAuditResponseDto>> GetAllAsync(AuditScope? scope, AuditStatus? status, int? officerId, string? sort)
    {
        try
        {
            var audits = await _context.Audits.ToListAsync();

            if (scope.HasValue)
                audits = audits.Where(a => a.Scope == scope.Value).ToList();

            if (status.HasValue)
                audits = audits.Where(a => a.Status == status.Value).ToList();

            if (officerId.HasValue)
                audits = audits.Where(a => a.OfficerID == officerId.Value).ToList();

            if (sort?.ToLower() == "asc")
                audits = audits.OrderBy(a => a.AuditID).ToList();
            else
                audits = audits.OrderByDescending(a => a.AuditID).ToList();

            return audits.Select(a => new CreateAuditResponseDto
            {
                AuditID = a.AuditID,
                OfficerID = a.OfficerID,
                Scope = a.Scope.ToString(),
                Findings = a.Findings,
                Date = a.Date,
                Status = a.Status.ToString()
            }).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception(ErrorMessages.Audit.FetchFailed, ex);
        }
    }

    /// <summary>
    /// Verifies that the given user exists and holds a valid field officer role (Police, Fire Fighter, or Emergency Dispatcher).
    /// </summary>
    /// <param name="officerId">The ID of the user to validate as a field officer.</param>
    /// <returns><c>true</c> if the user exists and has a valid officer role; otherwise, <c>false</c>.</returns>
    public async Task<bool> IsValidOfficerAsync(int officerId)
    {
        return await _context.Users
        .Include(u => u.UserRole)
        .AnyAsync(u => u.UserID == officerId &&
            (u.UserRole.RoleName == UserRoleOption.Police ||
             u.UserRole.RoleName == UserRoleOption.Fire_Fighter ||
             u.UserRole.RoleName == UserRoleOption.Emergency_Dispatcher));
    }
}
