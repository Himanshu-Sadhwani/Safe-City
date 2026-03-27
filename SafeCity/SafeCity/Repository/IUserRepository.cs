using SafeCity.DTOs;

namespace SafeCity.Repository
{
    public interface IUserRepository
    {
        public Task<UserRegisterResponseDto> RegisterUser(UserRegisterRequestDto request);
        public Task<UserUpdateByAdminResponseDto> UpdateUser(UserUpdateByAdminRequestDto request);
        public Task<ForgotPasswordResponseDto> ForgotPassword(ForgotPasswordRequestDto request);
    }
}
