using SafeCity.Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeCity.Domain.Entity
{
    public class Resource
    {
        [Key]
        public int ResourceID { get; set; }

        [Required(ErrorMessage = "Resource type is required")]
        [Column(TypeName = "varchar(20)")]
        public ResourceTypeOption Type { get; set; }

        [Required(ErrorMessage = "Availability status is required")]
        [Column(TypeName = "varchar(20)")]
        public ResourceAvailabilityOption Availability { get; set; }

        [Required(ErrorMessage = "Location is required")]
        [Column(TypeName = "varchar(max)")]
        public string Location { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string UnitName { get; set; }
    }
}