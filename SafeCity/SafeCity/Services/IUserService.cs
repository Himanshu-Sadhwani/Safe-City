using Microsoft.AspNetCore.Identity.Data;
using SafeCity.Domain.Entity;
using SafeCity.DTOs;
namespace SafeCity.Services
{
    public interface IUserService
    {
        public Task<UserRegisterResponseDto> RegisterUser(UserRegisterRequestDto request);
        Task<LoginResponseDto?> LoginUser(LoginRequest loginRequestDto);
        public Task<UserUpdateByAdminResponseDto> UpdateUser(UserUpdateByAdminRequestDto request);
        public Task<ForgotPasswordResponseDto> ForgotPassword(ForgotPasswordRequestDto request);
    }
}