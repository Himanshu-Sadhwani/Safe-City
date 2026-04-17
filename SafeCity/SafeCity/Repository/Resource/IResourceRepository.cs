using System;
using SafeCity.Domain.Entity;
using SafeCity.Domain.Enum;
using SafeCity.DTOs.Resource;
namespace SafeCity.Repository;
public interface IResourceRepository
{
    Task<IEnumerable<Resource>> GetAvailableResourcesAsync(ResourceTypeOption type);
    Task UpdateAsync(Resource resource);
    Task<List<GetResourceResponseDto>> ViewResources(int? resourceId, ResourceTypeOption? type, ResourceAvailabilityOption? availability, string? location, string? sortOrder);

}
