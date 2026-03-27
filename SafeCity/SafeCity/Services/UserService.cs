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

    /// <summary>
    /// Handles the forgot password operation by validating user input, hashing the new password,
    /// updating it in the database.
    /// </summary>
    /// <param name="request"> The forgot password request containing user email and the new password.</param>
    /// <returns> A response DTO confirming the password update.</returns>
    public async Task<ForgotPasswordResponseDto> ForgotPassword(
        ForgotPasswordRequestDto request)
    {
        // Check if the request exists
        if (request == null)
        {
            throw new ArgumentNullException(
                ErrorMessages.User.RequestNull);
        }

        // Validate required fields
        if (string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.PasswordHash))
        {
            throw new ArgumentException(
                ErrorMessages.User.RequiredFields);
        }

        // Validate email format
        var emailResult = EmailHelper.ValidateEmail(request.Email);
        if (!emailResult.IsValid)
        {
            throw new Exception(
                ErrorMessages.Validation.InvalidEmailFormat);
        }

        // Validate password strength
        var passwordResult =
            PasswordHelper.ValidatePassword(request.PasswordHash);
        if (!passwordResult.IsValid)
        {
            throw new Exception(
                ErrorMessages.Validation.WeakPassword);
        }

        // Hash the new password
        request.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash);
        // Update password in database
        return await _userRepository.ForgotPassword(request);
    }

}