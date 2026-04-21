using System;
using SafeCity.Domain.Enum;
using SafeCity.DTOs.Resource;

namespace SafeCity.Services.Resource;

public interface IResourceService
{   
    Task<List<GetResourceResponseDto>> ViewResources(int? resourceId, ResourceTypeOption? type, ResourceAvailabilityOption? availability, string? location, string? sortOrder);
}
