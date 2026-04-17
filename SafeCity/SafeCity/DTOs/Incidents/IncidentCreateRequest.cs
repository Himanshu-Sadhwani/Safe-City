using SafeCity.Domain.Entity;
using SafeCity.Domain.Enum;

namespace SafeCity.DTOs.Incidents
{
    public class IncidentCreateRequest
    {
        public int CitizenID { get; set; }
        public IncidentOption Type { get; set; }
        public string? Location { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public IncidentStatusOption Status { get; set; }

        public Incident ToEntity()
        {
            return new Incident
            {
                CitizenID = this.CitizenID,
                Type = this.Type,
                Location = this.Location,
                Date = this.Date,
                Status = IncidentStatusOption.Pending,
            };
        }
    }
}