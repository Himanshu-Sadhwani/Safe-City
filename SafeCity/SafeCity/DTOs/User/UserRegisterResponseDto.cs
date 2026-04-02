using SafeCity.Domain.Entity;
using SafeCity.Domain.Enum;
namespace SafeCity.DTOs
{
    public class UserRegisterResponseDto
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public UserStatus Status { get; set; }
        public string CurrentStatus { get; set; }
    }

    public static class UserResigterResponseExtension
    {
        public static UserRegisterResponseDto ToUserRegisterResponse(User response)
        {
            return new UserRegisterResponseDto()
            {
                UserID = response.UserID,
                Name = response.Name,
                RoleID = response.RoleID,
                RoleName = ((UserRoleOption)response.RoleID).ToString(),
                Email = response.Email,
                Phone = response.Phone,
                Status = response.Status,
                CurrentStatus = ((UserStatus)response.Status).ToString(),
            };
        }
    }
}
