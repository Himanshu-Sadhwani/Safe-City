using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SafeCity.Domain.Enums;

namespace SafeCity.Domain.Entity
{
    [Table("ComplianceRecord")]
    public class ComplianceRecord
    {
        [Key]
        public int ComplianceID { get; set; }
        [Required]
        public int EntityID { get; set; }
        [Required]
        public ComplianceType Type { get; set; }
        [Required]
        public ComplianceResult Result { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Column(TypeName = "TEXT")]
        public string? Notes { get; set; }
    }
}
