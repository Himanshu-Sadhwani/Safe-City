using SafeCity.Domain.Enum;
using SafeCity.DTOs.Case;
using SafeCity.Repository.Case;

namespace SafeCity.Services.Case;

public class CaseService : ICaseService
{
    private readonly ICaseRepository _repository;
    public CaseService(ICaseRepository repository)
    {
        _repository = repository;
    }

    // service layer logic for case creation 
    public async Task CreateCase(CaseCreation request)
    {
        try
        {
            // check if the request is null or not.
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            // check for the field validation
            var errorList = new List<string>();
            if (request.IncidentID <= 0)
            {
                errorList.Add("IncidentID must be greater than zero.");
            }
            if (request.AssignedOfficerID <= 0)
            {
                errorList.Add("AssignedOfficerID must be greater than zero.");
            }
            if (string.IsNullOrWhiteSpace(request.Description))
            {
                errorList.Add("Description is required.");
            }
            if (errorList.Any())
            {
                var errorMessage = string.Join(" | ", errorList);
                throw new Exception(errorMessage);
            }

            // if no field validation found call the repository layer to save the case details.
            await _repository.CreateCase(request);

        }
        catch (Exception ex)
        {
            // throws the error if any error is present
            throw new Exception(ex.Message);
        }
    }

    // service layer logic to view the case for admin and citizen
    public async Task<List<CaseResponse>> ViewCase(int userId, bool isAdmin, CaseStatusCheck? status, int? incidentId, DateTime? resolutionDate)
    {
        var response = await _repository.ViewCase(userId, isAdmin, status, incidentId, resolutionDate);
        return response;
    }
}
