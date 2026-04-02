using System;
using SafeCity.Domain.Entity;
using SafeCity.DTOs;

namespace SafeCity.Services.Auth;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginUser(LoginRequestDto loginRequestDto);
    string GenerateJwtToken(User user);
    string GenerateRefreshToken();
}
