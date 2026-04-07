using SafeCity.Domain.Enum;
using SafeCity.DTOs.Incidents;
using SafeCity.Repository.IncidentRepository;

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

        // View Incident Service layer logic based on the admin and citizen request and filter type
        public async Task<List<IncidentResponse>> ViewIncident(int userId, bool isAdmin, int incidentStatusType)
        {
            // response coming from the Repository Layer of view Incident 
            var response = await _incidentRepository.ViewIncident(userId, isAdmin, incidentStatusType);
            return response;
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