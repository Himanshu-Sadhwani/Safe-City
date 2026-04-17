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

    // case creation repository logic goes here
    public async Task CreateCase(CaseCreation request)
    {
        try
        {
            // check for the request is null or not
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            //check the incident id is valid or not 
            int incidentId = request.IncidentID;
            var checkIncident = await _context.Incidents.FindAsync(incidentId);
            if (checkIncident == null)
            {
                throw new Exception("No Incident Found.");
            }

            // check for duplicate case
            var existingCase = await _context.Cases.FirstOrDefaultAsync(c => c.IncidentID == request.IncidentID);
            if (existingCase != null)
            {
                throw new Exception("A case for this incident already exists.");
            }

            //check for the assigned officer id is a police officer or not.
            int assignedOfficerId = request.AssignedOfficerID;
            var checkForOfficer = await _context.Users.FindAsync(assignedOfficerId);
            if (checkForOfficer == null)
            {
                throw new Exception("No Officer Found.");
            }
            if (checkForOfficer != null)
            {
                int roleId = checkForOfficer.RoleID;
                if (roleId != 2)
                {
                    throw new Exception("Assigned Officer Id is not a valid Police officer Id");
                }
            }
            // try to save the case details to the database for further investigation
            request.Status = CaseStatusCheck.Open;
            var caseDetails = _mapper.Map<SafeCity.Domain.Entity.Case>(request);
            await _context.Cases.AddAsync(caseDetails);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    // view case repository logic goes here
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