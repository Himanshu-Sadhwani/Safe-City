using SafeCity.Domain.Entity;
using SafeCity.Domain.Enum;
using SafeCity.Domain.Entity;
using SafeCity.DTOs;

namespace SafeCity.Repository
{
    public interface IUserRepository
    {
        Task<ForgotPasswordResponseDto> ForgotPassword(ForgotPasswordRequestDto request);
        public Task<UserRegisterResponseDto> RegisterUser(UserRegisterRequestDto request);
        Task<User?> GetUserByEmailAndStatusAsync(string email, UserStatus status);
        Task SaveAuditLogAsync(int userId, string action);
        public Task<UserUpdateByAdminResponseDto> UpdateUser(int id,UserUpdateByAdminRequestDto request);
        public Task<User> GetUserByIdAsync(int userId);
        public Task<List<User>> GetAllUsersAsync();
    }
}
