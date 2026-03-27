using SafeCity.DTOs;

namespace SafeCity.Repository
{
    public interface IUserRepository
    {
        Task<ForgotPasswordResponseDto> ForgotPassword(ForgotPasswordRequestDto request);
        public Task<UserRegisterResponseDto> RegisterUser(UserRegisterRequestDto request);
        public Task<UserUpdateByAdminResponseDto> UpdateUser(UserUpdateByAdminRequestDto request);
    }
}
