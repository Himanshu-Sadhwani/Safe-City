using SafeCity.DTOs;

namespace SafeCity.Repository
{
    public interface IUserRepository
    {
        public Task<UserRegisterResponseDto> RegisterUser(UserRegisterRequestDto request);
    }
}
