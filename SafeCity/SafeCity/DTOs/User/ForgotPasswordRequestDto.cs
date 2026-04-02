using SafeCity.Domain.Entity;

namespace SafeCity.DTOs
{
    public class ForgotPasswordRequestDto
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        
        public void UpdateUserPassword(User user)
        {
            user.PasswordHash = PasswordHash;
        }
    }
}