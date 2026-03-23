using SafeCity.Domain.Entity;
using SafeCity.Domain.Enum;
namespace SafeCity.DTOs
{
    public class LoginResponseDto
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public UserRoleOption RoleName { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }

    public static class LoginResponseExtension
    {
        public static LoginResponseDto ToLoginResponse(User user, string token)
        {
            return new LoginResponseDto()
            {
                UserID = user.UserID,
                Name = user.Name,
                Email = user.Email,
                RoleName = user.UserRole.RoleName,
                Token = token,
                Expiration = DateTime.UtcNow.AddDays(1)

            };
        }
    }
}
