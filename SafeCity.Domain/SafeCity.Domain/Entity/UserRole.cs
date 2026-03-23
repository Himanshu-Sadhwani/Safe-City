using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using SafeCity.Domain.Enum;
namespace SafeCity.Domain.Entity
{
    public class UserRole
    {
        [Key]
        public int RoleID { get; set; }

        [Required]
        public UserRoleOption RoleName { get; set; }
    }
}
