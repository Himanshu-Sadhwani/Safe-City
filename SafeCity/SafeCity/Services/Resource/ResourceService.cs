
using SafeCity.Domain.Enum;
using SafeCity.DTOs.Resource;
using SafeCity.Repository;
namespace SafeCity.Services.Resource;
public class ResourceService : IResourceService
{
    private readonly IResourceRepository _resourceRepository;

    public ResourceService(IResourceRepository resourceRepository)
    {
        _resourceRepository = resourceRepository;
    }

    public async Task<List<GetResourceResponseDto>> ViewResources(
        int? resourceId,
        ResourceTypeOption? type,
        ResourceAvailabilityOption? availability,
        string? location,
        string? sortOrder)
    {
        try
        {
            return await _resourceRepository.ViewResources(
                resourceId,
                type,
                availability,
                location,
                sortOrder
            );
        }
        catch (Exception ex)
        {
            throw new Exception("Error while fetching resource details", ex);
        }
    }
}
