using SafeCity.Domain.Entity;

namespace SafeCity.DTOs
{
    public class ForgotPasswordRequestDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public void UpdateUserPassword(User user)
        {
            user.Password = Password;
        }
    }
}