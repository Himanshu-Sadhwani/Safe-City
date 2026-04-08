using SafeCity.Domain.Entity;
using SafeCity.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace SafeCity.DTOs.Incidents
{
    public class IncidentCreateRequest
    {
        [Required(ErrorMessage = "Citizen id is required")]
        public int CitizenID { get; set; }

        [Required(ErrorMessage = "Incident type is required")]
        public IncidentOption Type { get; set; } = default!;

        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Status is required")]
        public IncidentStatusOption Status { get; set; } = IncidentStatusOption.Pending;

        public Incident ToEntity()
        {
            return new Incident
            {
                CitizenID = this.CitizenID,
                Type = this.Type,
                Location = this.Location,
                Date = this.Date,
                Status = this.Status
            };
        }
    }
}