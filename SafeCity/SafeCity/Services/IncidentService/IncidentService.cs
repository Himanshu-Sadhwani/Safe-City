using SafeCity.Domain.Enum;
using SafeCity.DTOs.Incidents;
using SafeCity.Repository;

namespace SafeCity.Services.IncidentService
{
    public class IncidentService : IIncidentService
    {
        // dependency injection
        private readonly IIncidentRepository _incidentRepository;

        public IncidentService(IIncidentRepository _incidentRepository)
        {
            this._incidentRepository = _incidentRepository;
        }

        // Submit incident service layer logic
        public async Task SubmitIncident(IncidentCreateRequest request)
        {
            // check if the request is null 
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Request data cannot be null");
            }
            // calling of methods to validate or the required request field
            var errors = ValidateRequest(request);
            // throws the error if there is any error present
            if (errors.Any())
            {
                throw new ArgumentException(string.Join(" | ", errors));
            }

            // calling the next Incident Repository layer to save the request to the database
            await _incidentRepository.SubmitIncident(request);
        }

        // Field Validation Helper
        private List<string> ValidateRequest(IncidentCreateRequest request)
        {
            // dynamic collection which stores all the error
            var errorList = new List<string>();

            if (request.CitizenID <= 0)
                errorList.Add("Invalid Citizen Id");

            if (!Enum.IsDefined(typeof(IncidentOption), request.Type))
                errorList.Add("Invalid Incident Type");

            if (string.IsNullOrWhiteSpace(request.Location))
                errorList.Add("Location is required");

            if (request.Date > DateTime.Now)
                errorList.Add("Incident Date cannot be in the future");

            if (!Enum.IsDefined(typeof(IncidentStatusOption), request.Status))
                errorList.Add("Invalid Incident Status");

            // return the errorList i.e. null or the errorList
            return errorList;
        }
    }
}