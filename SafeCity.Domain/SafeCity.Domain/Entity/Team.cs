using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SafeCity.Domain.Entity;

[Table("Team")]
public class Team
{
    [Key]
    public int TeamID { get; set; }
    [Required, MaxLength(100)]
    public string TeamName { get; set; } = default!;
    [Required]
    public int TeamLeadID { get; set; }
    [Required]
    public TeamStatus Status { get; set; } = TeamStatus.Active;
    [ForeignKey(nameof(TeamLeadID))]
    public virtual User TeamLead { get; set; } = default!;
    public virtual ICollection<Response> Responses { get; set; } = new List<Response>();

}

