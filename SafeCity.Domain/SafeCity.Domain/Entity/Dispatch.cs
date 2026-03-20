using SafeCity.Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeCity.Domain.Entity
{
    public class Dispatch
    {
        [Key]
        public int DispatchID { get; set; }

        [Required(ErrorMessage = "Incident id is required")]
        public int IncidentID { get; set; }

        [Required(ErrorMessage = "Dispatcher id is required")]
        public int DispatcherID { get; set; }

        public int? ResourceID { get; set; }

        [Required(ErrorMessage = "Dispatch status is required")]
        [Column(TypeName = "varchar(20)")]
        public DispatchStatusOption Status { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(100)")]
        public DateTime Date { get; set; } = DateTime.UtcNow;

        [ForeignKey("IncidentID")]
        public virtual Incident Incident { get; set; }

        [ForeignKey("DispatcherID")]
        public virtual User User { get; set; }

        [ForeignKey("ResourceID")]
        public virtual Resource Resource { get; set; }
    }
}