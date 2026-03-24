using Microsoft.EntityFrameworkCore;
using SafeCity.Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SafeCity.Domain.Entity;

[Index(nameof(Email), IsUnique = true)]
public class User
{
    [Key]
    public int UserID { get; set; }

    [Column(TypeName = "VARCHAR(MAX)")]
    public string PasswordHash { get; set; } = default!;

    [Column(TypeName = "VARCHAR(MAX)")]
    public string PasswordSalt { get; set; } = default!;

    [Required]
    [Column(TypeName = "VARCHAR(100)")]
    public string Name { get; set; } = default!;

    [Required]
    public int RoleID { get; set; }

    [Required]
    [Column(TypeName = "VARCHAR(254)")]
    public string Email { get; set; } = default!;

    [Column(TypeName = "VARCHAR(20)")]
    public string Phone { get; set; }

    [Required]
    [Column(TypeName = "VARCHAR(20)")]
    public UserStatus Status { get; set; }


    [ForeignKey("RoleID")]
    public virtual UserRole UserRole { get; set; }


    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
}