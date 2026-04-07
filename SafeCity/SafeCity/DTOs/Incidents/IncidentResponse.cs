using SafeCity.Domain.Entity;

namespace SafeCity.DTOs.Incidents
{
    // Properties that will be visible to the user as a response
    public class IncidentResponse
    {
        public int IncidentID { get; set; }
        public int CitizenID { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
    }

    // extension class for the IncidentResponse and is used to map the entity to the dto
    public static class IncidentResponseExtension
    {
        public static IncidentResponse ToIncidentResponse(Incident incident)
        {
            return new IncidentResponse
            {
                IncidentID = incident.IncidentID,
                CitizenID = incident.CitizenID,
                Type = incident.Type.ToString(),
                Location = incident.Location,
                Date = incident.Date,
                Status = incident.Status.ToString(),
            };
        }
    }
}
