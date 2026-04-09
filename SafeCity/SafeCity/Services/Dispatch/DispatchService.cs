using SafeCity.Domain.Enum;
using SafeCity.DTOs;
using SafeCity.Repository;
using SafeCity.Domain.Entity;
using SafeCity.Utility;

namespace SafeCity.Services.Dispatch
{
    /// <summary>
    /// Service responsible for handling dispatch operations,
    /// including assigning one or more available resources to incidents.
    /// </summary>
    public class DispatchService : IDispatchService
    {
        private readonly IIncidentRepository _incidentRepository;
        private readonly IResourceRepository _resourceRepository;
        private readonly IDispatchRepository _dispatchRepository;
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DispatchService"/> class.
        /// </summary>
        public DispatchService(
            IIncidentRepository incidentRepository,
            IResourceRepository resourceRepository,
            IDispatchRepository dispatchRepository,
            IUserRepository userRepository)
        {
            _incidentRepository = incidentRepository;
            _resourceRepository = resourceRepository;
            _dispatchRepository = dispatchRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Assigns an available resource unit to an incident.
        /// Multiple resources can be assigned to the same incident.
        /// </summary>
        /// <param name="request">Dispatch request containing incident and dispatcher details.</param>
        /// <returns>A <see cref="DispatchResponseDto"/> with dispatch details.</returns>
        /// <exception cref="Exception">
        /// Thrown when validation fails or resources are unavailable.
        /// </exception>
        public async Task<DispatchResponseDto> AssignUnitAsync(DispatchRequestDto request)
        {
            var errorList = new List<string>();

            if (request == null)
                errorList.Add(ErrorMessages.Dispatch.RequestNull);
            else
            {
                if(request.IncidentId==0)
                    errorList.Add(ErrorMessages.Dispatch.IncidentIdRequired);

                if(request.DispatcherId==0)
                    errorList.Add(ErrorMessages.Dispatch.DispatcherIdRequired);

                if (request.IncidentId <0)
                    errorList.Add(ErrorMessages.Dispatch.InvalidIncidentId);

                if (request.DispatcherId <0)
                    errorList.Add(ErrorMessages.Dispatch.InvalidDispatcherId);
            }

            // Stop early if request itself is invalid
            if (errorList.Any())
                throw new ArgumentException(string.Join(" | ", errorList));

            var incident = await _incidentRepository.GetByIdAsync(request.IncidentId);
            if (incident == null)
                errorList.Add(ErrorMessages.Dispatch.IncidentNotFound);

            var dispatcher = await _userRepository.GetUserByIdAsync(request.DispatcherId);

            if (dispatcher == null)
                errorList.Add(ErrorMessages.Dispatch.DispatcherNotFound);
            else if (dispatcher.Status != UserStatus.Active)
                errorList.Add(ErrorMessages.Dispatch.DispatcherInactive);

            // Stop if core entities are invalid
            if (errorList.Any())
                throw new ArgumentException(string.Join(" | ", errorList));

            ResourceTypeOption resourceType;
            try
            {
                resourceType = MapIncidentToResourceType(incident!.Type);
            }
            catch
            {
                throw new Exception(ErrorMessages.Dispatch.InvalidIncidentType);
            }

            var availableResources =
                await _resourceRepository.GetAvailableResourcesAsync(resourceType);

            if (!availableResources.Any())
                throw new Exception(ErrorMessages.Dispatch.NoAvailableResources);

            var existingDispatches =
                await _dispatchRepository.GetByIncidentIdAsync(incident!.IncidentID);

            var selectedResource = availableResources
                .FirstOrDefault(r => !existingDispatches.Any(d => d.ResourceID == r.ResourceID));

            if (selectedResource == null)
                throw new Exception(ErrorMessages.Dispatch.ResourceAlreadyAssigned);

            var dispatch = new Domain.Entity.Dispatch
            {
                IncidentID = incident.IncidentID,
                DispatcherID = dispatcher!.UserID,
                ResourceID = selectedResource.ResourceID,
                Status = DispatchStatusOption.Assigned,
                Date = DateTime.UtcNow
            };

            try
            {
                await _dispatchRepository.AddAsync(dispatch);
            }
            catch
            {
                throw new Exception(ErrorMessages.Dispatch.DispatchCreationFailed);
            }

            selectedResource.Availability = ResourceAvailabilityOption.OnTask;

            try
            {
                await _resourceRepository.UpdateAsync(selectedResource);
            }
            catch
            {
                throw new Exception(ErrorMessages.Dispatch.ResourceUpdateFailed);
            }

            if (incident.Status == IncidentStatusOption.Pending)
            {
                incident.Status = IncidentStatusOption.InProgress;

                try
                {
                    await _incidentRepository.UpdateAsync(incident);
                }
                catch
                {
                    throw new Exception(ErrorMessages.Dispatch.IncidentUpdateFailed);
                }
            }

            return new DispatchResponseDto
            {
                DispatchId = dispatch.DispatchID,
                IncidentId = incident.IncidentID,
                ResourceId = selectedResource.ResourceID,
                UnitName = selectedResource.UnitName,
                Status = dispatch.Status,
                DispatchDateTime = dispatch.Date
            };
        }


        /// <summary>
        /// Maps an incident type to the appropriate resource type.
        /// </summary>
        private ResourceTypeOption MapIncidentToResourceType(IncidentOption incidentType)
        {
            return incidentType switch
            {
                IncidentOption.Crime    => ResourceTypeOption.Vehicle,
                IncidentOption.Fire     => ResourceTypeOption.FireTruck,
                IncidentOption.Accident => ResourceTypeOption.Ambulance,
                IncidentOption.Other    => ResourceTypeOption.Equipment,
                _ => throw new Exception()
            };
        }
    }
}
