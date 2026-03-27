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
    /// It validates all fields at once and returns a combined error message if any fail.
    /// </summary>
    /// <param name="request">The data provided for registration.</param>
    /// <returns>The result of the registration process.</returns>
    public async Task<UserRegisterResponseDto> RegisterUser(UserRegisterRequestDto request)
    {
        // Check if the request exists
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), ErrorMessages.User.RequestNull);
        }

        // Create a list to collect all validation errors
        var errorList = new List<string>();

        // Make sure all required information is filled in
        if (string.IsNullOrWhiteSpace(request.Name))
            errorList.Add(ErrorMessages.User.Field.GetValueOrDefault(nameof(request.Name), "Name is missing."));

        if (string.IsNullOrWhiteSpace(request.Email))

            errorList.Add(ErrorMessages.User.Field.GetValueOrDefault(nameof(request.Email), "Email is missing."));

        if (string.IsNullOrWhiteSpace(request.PasswordHash))
            errorList.Add(ErrorMessages.User.Field.GetValueOrDefault(nameof(request.PasswordHash), "Password is missing."));

        if (string.IsNullOrWhiteSpace(request.Phone))
            errorList.Add(ErrorMessages.User.Field.GetValueOrDefault(nameof(request.Phone), "Phone is missing."));

        if (request.RoleID <= 0)
            errorList.Add(ErrorMessages.User.Field.GetValueOrDefault(nameof(request.RoleID), "Invalid Role."));

        // Validate that the email format is correct (only if email was provided)
        if (!string.IsNullOrWhiteSpace(request.Email) && EmailHelper.ValidateEmail(request.Email).IsValid)
        {
            var message = EmailHelper.ValidateEmail(request.Email);
            errorList.Add(message.Message);
        }

        // Validate that the password meets security rules (only if password was provided)
        if (!string.IsNullOrWhiteSpace(request.PasswordHash) && !PasswordHelper.ValidatePassword(request.PasswordHash).IsValid)
        {
            var message = PasswordHelper.ValidatePassword(request.PasswordHash);
            errorList.Add(message.Message);
        }

        // If any validation failed, throw all errors separated by a pipe
        if (errorList.Any())
        {
            throw new ArgumentException(string.Join(" | ", errorList));
        }

        // Hash the password to keep it safe in the database
        request.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash);

        // Pass the data to the repository to be saved
        var response = await _userRepository.RegisterUser(request);

        // Ensure the response from the database is valid
        if (response == null)
        {
            throw new Exception(ErrorMessages.Database.SaveFailed);
        }

        return response;
    }
}