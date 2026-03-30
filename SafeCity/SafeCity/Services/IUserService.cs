using SafeCity.DTOs;

namespace SafeCity.Services
{
    public interface IUserService
    {
        public Task<UserRegisterResponseDto> RegisterUser(UserRegisterRequestDto request);
        public Task<ViewOneUserResponseDto> GetUserByIdAsync(int userId);
        public Task<List<ViewAllUsersResponseDto>> GetAllUsersAsync();
        public Task<UserUpdateByAdminResponseDto> UpdateUser(UserUpdateByAdminRequestDto request);
        public Task<ForgotPasswordResponseDto> ForgotPassword(ForgotPasswordRequestDto request);

    }
}
