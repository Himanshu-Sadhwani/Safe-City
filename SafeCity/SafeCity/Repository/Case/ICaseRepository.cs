using SafeCity.Domain.Enum;
using SafeCity.DTOs.Case;
namespace SafeCity.Repository.Case;

public interface ICaseRepository
{
    public Task<List<CaseResponse>> ViewCase(int userId, bool isAdmin, CaseStatusCheck? status, int? incidentId, DateTime? resolutionDate);
    public Task CreateCase(CaseCreation request);
}
