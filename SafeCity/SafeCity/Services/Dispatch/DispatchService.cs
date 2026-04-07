using SafeCity.Domain.Enum;
using SafeCity.DTOs;
using SafeCity.Repository;
using  SafeCity.Domain.Entity;
namespace SafeCity.Services.Dispatch
{
    public class DispatchService : IDispatchService
    {
        private readonly IIncidentRepository _incidentRepository;
        private readonly IResourceRepository _resourceRepository;
        private readonly IDispatchRepository _dispatchRepository;

        public DispatchService(
            IIncidentRepository incidentRepository,
            IResourceRepository resourceRepository,
            IDispatchRepository dispatchRepository)
        {
            _incidentRepository = incidentRepository;
            _resourceRepository = resourceRepository;
            _dispatchRepository = dispatchRepository;
        }

        public async Task<DispatchResponseDto> AssignUnitAsync(DispatchRequestDto request)
        {
            var incident = await _incidentRepository.GetByIdAsync(request.IncidentId);
            if (incident == null)
                throw new Exception("Incident not found");

            if (await _dispatchRepository.DispatchExistsForIncidentAsync(incident.IncidentID))
                throw new Exception("Incident already dispatched");

            var resourceType = MapIncidentToResourceType(incident.Type);

            var availableResources = await _resourceRepository.GetAvailableResourcesAsync(resourceType);
            if (!availableResources.Any())
                throw new Exception("No available resources");

            var nearestResource = availableResources
                .OrderBy(r => r.Location == incident.Location ? 0 : 1)
                .First();

            var dispatch = new SafeCity.Domain.Entity.Dispatch
            {
                IncidentID = incident.IncidentID,
                DispatcherID = request.DispatcherId,
                ResourceID = nearestResource.ResourceID,
                Status = DispatchStatusOption.Assigned,
                Date = DateTime.UtcNow
            };
            await _dispatchRepository.AddAsync(dispatch);

            nearestResource.Availability = ResourceAvailabilityOption.OnTask;
            incident.Status = IncidentStatusOption.InProgress;

            await _resourceRepository.UpdateAsync(nearestResource);
            await _incidentRepository.UpdateAsync(incident);

            return await Task.FromResult(new DispatchResponseDto
            {
                DispatchId = dispatch.DispatchID,
                IncidentId = incident.IncidentID,
                ResourceId = nearestResource.ResourceID,
                UnitName = nearestResource.UnitName,
                Status = dispatch.Status,
                DispatchDateTime = dispatch.Date 
            });
        }

        private ResourceTypeOption MapIncidentToResourceType(IncidentOption incidentType)
        {
            return incidentType switch
            {
                IncidentOption.Crime    => ResourceTypeOption.Vehicle,
                IncidentOption.Fire     => ResourceTypeOption.FireTruck,
                IncidentOption.Accident => ResourceTypeOption.Ambulance,
                IncidentOption.Other    => ResourceTypeOption.Equipment,
                _                       => ResourceTypeOption.Vehicle
            };
        }
    }
}