using SafeCity.Domain.Entity;
using SafeCity.Domain.Enum;

namespace SafeCity.DTOs
{
    public class UserUpdateByAdminResponseDto
    {
        public int UserID { get; init; }
        public string Name { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string Email { get; init; }
        public string Phone { get; set; }
        public UserStatus Status { get; set; }
        public string CurrentStatus { get; set; }
    }

    public static class UserUpdateByAdminResponseExtension
    {
        public static UserUpdateByAdminResponseDto ToUserUpdateByAdminResponse(this User user)
        {
            return new  UserUpdateByAdminResponseDto
            {
                UserID = user.UserID,
                Name = user.Name,
                RoleID = user.RoleID,
                RoleName = ((UserRoleOption)user.RoleID).ToString(),
                Email = user.Email,
                Phone = user.Phone,
                Status = user.Status,
                CurrentStatus = ((UserStatus)user.Status).ToString()
            };
        }
    }
}
