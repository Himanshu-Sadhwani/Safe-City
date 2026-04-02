using SafeCity.Domain.Entity;
using SafeCity.Domain.Enum;
namespace SafeCity.DTOs
{
    public class UserUpdateByAdminRequestDto
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public int RoleID { get; set; }
        public UserStatus Status { get; set; } 
    }
}