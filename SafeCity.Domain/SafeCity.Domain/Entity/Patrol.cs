using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SafeCity.Domain.Enum;

namespace SafeCity.Domain.Entity;

[Table("Patrol")]
public class Patrol
{
    [Key]
    public int PatrolId { get; set; }

    [ForeignKey(nameof(User))]
    [Required(ErrorMessage = "OfficerId can't be null")]
    public int OfficerId { get; set; }

    [Column(TypeName = "VARCHAR(100)")]
    public string Area { get; set; }

    public DateTime Date { get; set; }

    [Column(TypeName = "VARCHAR(20)")]
    public PatrolStatus Status { get; set; }

    public virtual User User { get; set; }

    public virtual ICollection<FieldReport> FieldReports { get; set; } = new List<FieldReport>();
}
