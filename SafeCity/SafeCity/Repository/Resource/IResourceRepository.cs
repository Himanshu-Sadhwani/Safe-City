using System;
using SafeCity.Domain.Entity;
using SafeCity.Domain.Enum;
namespace SafeCity.Repository;
public interface IResourceRepository
{
    Task<IEnumerable<Resource>> GetAvailableResourcesAsync(ResourceTypeOption type);
    Task UpdateAsync(Resource resource);
}
