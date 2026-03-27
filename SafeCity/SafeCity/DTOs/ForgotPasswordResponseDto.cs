using SafeCity.Domain.Entity;

namespace SafeCity.DTOs
{
    public class ForgotPasswordResponseDto
    {
        public int UserID { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }

    public static class ForgotPasswordResponseExtension
    {
        public static ForgotPasswordResponseDto ToForgotPasswordResponse(this User user)
        {
            return new ForgotPasswordResponseDto
            {
                UserID = user.UserID,
                Email = user.Email,
                Message = "Password updated successfully."
            };
        }
    }
}