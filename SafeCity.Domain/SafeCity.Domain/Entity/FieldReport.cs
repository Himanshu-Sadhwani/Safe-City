using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SafeCity.Domain.Enum;

namespace SafeCity.Domain.Entity;

[Table("FieldReport")]
public class FieldReport
{
    [Key]
    public int ReportId { get; set; }
    
    [ForeignKey(nameof(Patrol))]
    [Required(ErrorMessage ="PatrolId can't be null")]
    public int PatrolId { get; set; }

    [Column(TypeName ="VARCHAR(100)")]
    public string Notes { get; set; }

    public DateTime Date { get; set; }

    [Column(TypeName ="VARCHAR(20)")]
    public FieldReportStatus Status { get; set; }

    public virtual Patrol Patrol {get; set;}
}