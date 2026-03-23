using SafeCity.Domain.Entity;
using SafeCity.Domain.Enum;

namespace SafeCity.DTOs
{
    public class UserResponseDto
    {
        public int UserID { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string Name { get; set; }
        public int RoleID { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public UserStatus Status { get; set; }
        public virtual UserRole UserRole { get; set; }
    }

    public static class UserResponseExtension
    {
        public static UserResponseDto ToUserResponse(User user)
        {
            return new UserResponseDto()
            {
                UserID = user.UserID,
                PasswordHash = user.PasswordHash,
                PasswordSalt = user.PasswordSalt,
                Name = user.Name,
                RoleID = user.RoleID,
                Email = user.Email,
                Phone = user.Phone,
                Status = user.Status,
                UserRole = user.UserRole
            };
        }
    }
}
