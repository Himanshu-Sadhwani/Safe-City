using System;
using SafeCity.Utility;
using SafeCity.DTOs;
using SafeCity.Domain.Data;

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
        catch (Exception)
        {
            throw new Exception(ErrorMessages.Audit.SaveFailed);
        }
    }
}
