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

    // service layer logic to view the case for admin and citizen
    public async Task<List<CaseResponse>> ViewCase(int userId, bool isAdmin, CaseStatusCheck? status, int? incidentId, DateTime? resolutionDate)
    {
        var response = await _repository.ViewCase(userId, isAdmin, status, incidentId, resolutionDate);
        return response;
    }
}
