using SafeCity.Domain.Entity;
using SafeCity.DTOs;

namespace SafeCity.Repository
{
    public interface IUserRepository
    {
        public Task<UserRegisterResponseDto> RegisterUser(UserRegisterRequestDto request);
        public Task<User> GetUserByIdAsync(int userId);
        public Task<List<User>> GetAllUsersAsync();
         public Task<UserUpdateByAdminResponseDto> UpdateUser(UserUpdateByAdminRequestDto request);
        Task<ForgotPasswordResponseDto> ForgotPassword(ForgotPasswordRequestDto request);
    }
}
