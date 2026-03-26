using SafeCity.DTOs;
using SafeCity.Repository;
using SafeCity.Utility;
namespace SafeCity.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserRegisterResponseDto> RegisterUser(UserRegisterRequestDto request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }
        if (request.PasswordHash == null || request.Email == null ||
            request.Name == null || request.Phone == null || request.RoleID == null)
        {
            throw new ArgumentException("Required fields are missing.");
        }
        var emailResult = EmailHelper.ValidateEmail(request.Email);
        if (!emailResult.IsValid)
        {
            throw new Exception(emailResult.Message);
        }
        var passwordResult = PasswordHelper.ValidatePassword(request.PasswordHash);
        if (!passwordResult.IsValid)
        {
            throw new Exception(passwordResult.Message);
        }
        request.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash);
        var response = await _userRepository.RegisterUser(request);

        return response;
    }

    public async Task<UserUpdateByAdminResponseDto> UpdateUserByAdmin(UserUpdateByAdminRequestDto request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if (request.UserID <= 0)
            throw new ArgumentException("Invalid UserID.");

        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Name is required.");

        if (string.IsNullOrWhiteSpace(request.Phone))
            throw new ArgumentException("Phone is required.");

        if (request.RoleID <= 0)
            throw new ArgumentException("Invalid RoleID.");
        return await _userRepository.UpdateUserByAdmin(request);

    }
}