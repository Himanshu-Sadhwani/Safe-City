using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SafeCity.Domain.Enum;

namespace SafeCity.Domain.Entity
{
    [Table("Audit")]
    public class Audit
    {
        [Key]
        public int AuditID { get; set; }
        [Required]
        [ForeignKey(nameof(Officer))]
        public int OfficerID { get; set; }
        [Required]
        public AuditScope Scope { get; set; }
        [Required]
        [Column(TypeName = "TEXT")]
        public string Findings { get; set; }
        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;
        [Required]
        public AuditStatus Status { get; set; }

        public virtual User? Officer { get; set; }
    }
}
