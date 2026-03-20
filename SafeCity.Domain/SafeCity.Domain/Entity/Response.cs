using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SafeCity.Domain.Entity;
[Table("Response")]
    public class Response
    {
        [Key]
        public int ResponseID { get; set; }
        [Required]
        public int CrisisID { get; set; }
        [Required]
        public int TeamID { get; set; }
        [Required, MaxLength(1000)]
        public string Actions { get; set; } = default!;
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public ResponseStatus Status { get; set; } = ResponseStatus.Pending;
        [ForeignKey(nameof(CrisisID))]
        public virtual required Crisis CrisisIdNavigation { get; set; }
        [ForeignKey(nameof(TeamID))]
        public virtual required Team TeamIdNavigation { get; set; }
}

