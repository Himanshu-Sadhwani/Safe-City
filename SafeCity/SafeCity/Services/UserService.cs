using System;
using SafeCity.DTOs;
using SafeCity.Repository;
namespace SafeCity.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public Task<UserResponseDto> CreateUser(UserRequestDto userRequestDto)
    {
        var response= _userRepository.CreateUser(userRequestDto);
        return response;
    }

    public Task<bool> DeleteUser(int id)
    {
        var response = _userRepository.DeleteUser(id);
        return response;
    }
    
    public Task<IEnumerable<UserResponseDto>> GetAllUsers()
    {
        var response = _userRepository.GetAllUsers();
        return response;
    }

    public Task<UserResponseDto> GetUserById(int id)
    {
        var response = _userRepository.GetUserById(id);
        return response;
    }
    

    public Task<LoginResponseDto> LoginUser(LoginRequestDto loginRequestDto)
    {
        var response = _userRepository.LoginUser(loginRequestDto);
        return response;
    }

    public Task<IEnumerable<UserResponseDto>> SearchUsers(string searchTerm)
    {
        var response = _userRepository.SearchUsers(searchTerm);
        return response;
    }

    public Task<UserResponseDto> UpdateUser(int id, UserRequestDto userRequestDto)
    {
        var response = _userRepository.UpdateUser(id, userRequestDto);
        return response;
    }

    public Task<bool> UpdateUserStatus(int id, bool isActive)
    {
        var response = _userRepository.UpdateUserStatus(id, isActive);
        return response;
    }
}
