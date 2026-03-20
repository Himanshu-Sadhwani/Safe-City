using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeCity.Domain.Entity;

[Table("AuditLog")]
public class AuditLog
{
        [Key]
        public int AuditID { get; set; } = default!;

        [Required]
        public int UserID { get; set; } = default!;

        [Required]
        [Column(TypeName = "VARCHAR(50)")]
        public string Action { get; set; } = default!;

        [Required]
        [Column(TypeName = "VARCHAR(100)")]
        public string Resource { get; set; } = default!;

        [Required]
        [Column(TypeName = "DATETIME")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}