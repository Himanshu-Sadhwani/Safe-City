using System;
using SafeCity.Utility;
using SafeCity.DTOs;
using SafeCity.Domain.Data;
using Microsoft.EntityFrameworkCore;

namespace SafeCity.Repository.Audit;

public class AuditRepository : IAuditRepository
{
    private readonly SafeCityDbContext _context;

    public AuditRepository(SafeCityDbContext context)
    {
        _context = context;
    }
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
