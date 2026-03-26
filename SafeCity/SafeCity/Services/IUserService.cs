using SafeCity.Domain.Entity;
using SafeCity.DTOs;

public interface IUserService
{
    Task<LoginResponseDto?> LoginUser(LoginRequestDto loginRequestDto);
    Task SaveAuditLog(int userId, string action);
    string GenerateRefreshToken();
    string GenerateJwtToken(User user);
}