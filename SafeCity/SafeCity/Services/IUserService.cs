using SafeCity.DTOs;

namespace SafeCity.Services
{
    public interface IUserService
    {
        public Task<UserUpdateByAdminResponseDto> UpdateUserByAdmin(UserUpdateByAdminRequestDto request);

    }
}
