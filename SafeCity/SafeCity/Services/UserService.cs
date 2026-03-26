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
            throw new ArgumentNullException(ErrorMessages.User.RequestNull);
        }


        // Make sure all required information is filled in
        if (string.IsNullOrWhiteSpace(request.PasswordHash) ||
            string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Name) ||
            string.IsNullOrWhiteSpace(request.Phone) ||
            request.RoleID <= 0)
        {
            throw new ArgumentException(ErrorMessages.User.RequiredFields);
        }

        // Validate that the email format is correct
        var emailResult = EmailHelper.ValidateEmail(request.Email);
        if (!emailResult.IsValid)
        {
            throw new Exception(ErrorMessages.Validation.InvalidEmailFormat);
        }

        // Validate that the password meets security rules
        var passwordResult = PasswordHelper.ValidatePassword(request.PasswordHash);
        if (!passwordResult.IsValid)
        {
            throw new Exception(ErrorMessages.Validation.WeakPassword);
        }

        // Hash the password to keep it safe in the database
        request.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash);
        // Pass the data to the repository to be saved
        var response = await _userRepository.RegisterUser(request);

        return response;
    }
}