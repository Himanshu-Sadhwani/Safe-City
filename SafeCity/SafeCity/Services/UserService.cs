using SafeCity.DTOs;
using SafeCity.Repository;
using SafeCity.Utility;

namespace SafeCity.Services;

/// <summary>
/// This service handles the logic for user registration, like validation and security.
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /// <summary>
    /// Checks the user's data, hashes the password, and saves the user to the database.
    /// </summary>
    /// <param name="request">The data provided for registration.</param>
    /// <returns>The result of the registration process.</returns>
    public async Task<UserRegisterResponseDto> RegisterUser(UserRegisterRequestDto request)
    {
        // Check if the request exists
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        // Make sure all required information is filled in
        if (request.PasswordHash == null || request.Email == null ||
            request.Name == null || request.Phone == null || request.RoleID == null)
        {
            throw new ArgumentException("Required fields are missing.");
        }

        // Validate that the email format is correct
        var emailResult = EmailHelper.ValidateEmail(request.Email);
        if (!emailResult.IsValid)
        {
            throw new Exception(emailResult.Message);
        }

        // Validate that the password meets security rules
        var passwordResult = PasswordHelper.ValidatePassword(request.PasswordHash);
        if (!passwordResult.IsValid)
        {
            throw new Exception(passwordResult.Message);
        }

        // Hash the password to keep it safe in the database
        request.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash);

        // Pass the data to the repository to be saved
        var response = await _userRepository.RegisterUser(request);

        return response;
    }
}