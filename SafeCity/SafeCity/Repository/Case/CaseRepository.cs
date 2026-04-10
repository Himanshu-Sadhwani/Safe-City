using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SafeCity.Domain.Data;
using SafeCity.Domain.Enum;
using SafeCity.DTOs.Case;

namespace SafeCity.Repository.Case;

public class CaseRepository : ICaseRepository
{
    private readonly SafeCityDbContext _context;
    private readonly IMapper _mapper;

    public CaseRepository(SafeCityDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    // view case repository loigc goes here
    public async Task<List<CaseResponse>> ViewCase(int userId, bool isAdmin, CaseStatusCheck? status, int? incidentId, DateTime? resolutionDate)
    {
        try
        {   // find all the case list with its incident details
            var casesList = await _context.Cases
                .Include(c => c.Incident)
                .ToListAsync();

            // check if the user is admin or not
            if (!isAdmin)
            {
                casesList = casesList.Where(temp => temp.Incident.CitizenID == userId).ToList();
            }

            // filteration logic goes here
            if (status.HasValue)
            {
                casesList = casesList.Where(temp => temp.Status == status.Value).ToList();
            }
            if (incidentId.HasValue)
            {
                casesList = casesList.Where(temp => temp.IncidentID == incidentId.Value).ToList();
            }
            if (resolutionDate.HasValue)
            {
                casesList = casesList.Where(temp => temp.ResolutionDate.Date == resolutionDate.Value.Date).ToList();
            }
            // displaying the latest Case Details at the top
            casesList = casesList.OrderByDescending(temp => temp.CaseID).ToList();

            return _mapper.Map<List<CaseResponse>>(casesList);
        }
        catch (Exception ex)
        {
            // throws the error if any present
            throw new Exception(ex.Message);
        }
    }
}