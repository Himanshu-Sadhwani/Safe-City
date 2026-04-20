using SafeCity.Domain.Enum;
using SafeCity.DTOs.Case;

namespace SafeCity.Services.Case;

public interface ICaseService
{
    public Task<List<CaseResponse>> ViewCase(int userId, bool isAdmin, CaseStatusCheck? status, int? incidentId, DateTime? resolutionDate);
    public Task CreateCase(CaseCreation request);
}
