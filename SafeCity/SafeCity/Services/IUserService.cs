using System;
using SafeCity.DTOs;
namespace SafeCity.Services;
public interface IUserService
{

    public Task<UserResponseDto> CreateUser(UserRequestDto userRequestDto);
    public Task<LoginResponseDto> LoginUser(LoginRequestDto loginRequestDto);
    public Task<UserResponseDto> GetUserById(int id);
    public Task<IEnumerable<UserResponseDto>> GetAllUsers();
    public Task<IEnumerable<UserResponseDto>> SearchUsers(string searchTerm);
    public Task<UserResponseDto> UpdateUser(int id, UserRequestDto userRequestDto);
    public Task<bool> UpdateUserStatus(int id, bool isActive);
    public Task<bool> DeleteUser(int id);
}
