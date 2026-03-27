using SafeCity.Domain.Entity;
using SafeCity.DTOs;
namespace SafeCity.Services
{
    public interface IUserService
    {
        public Task<UserRegisterResponseDto> RegisterUser(UserRegisterRequestDto request);
        Task<LoginResponseDto?> LoginUser(LoginRequestDto loginRequestDto);
        Task SaveAuditLog(int userId, string action);
        string GenerateRefreshToken();
        string GenerateJwtToken(User user);
    }
}
