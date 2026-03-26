using SafeCity.DTOs;

namespace SafeCity.Repository
{
    public interface IUserRepository
    {
         public Task<UserUpdateByAdminResponseDto> UpdateUserByAdmin(UserUpdateByAdminRequestDto request);
    }
}
