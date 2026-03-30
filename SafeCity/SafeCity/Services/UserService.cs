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
        //Initial Null Check
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), ErrorMessages.User.RequestNull);
        }

        var errorList = new List<string>();
        var fields = ErrorMessages.User.Field;

        // Required Field Validations
        var nameError = ValidationHelper.CheckNullOrWhiteSpace(request.Name, nameof(request.Name), fields);
        if (nameError != null) errorList.Add(nameError);

        var emailError = ValidationHelper.CheckNullOrWhiteSpace(request.Email, nameof(request.Email), fields);
        if (emailError != null) errorList.Add(emailError);

        var passwordError = ValidationHelper.CheckNullOrWhiteSpace(request.PasswordHash, nameof(request.PasswordHash), fields);
        if (passwordError != null) errorList.Add(passwordError);

        var phoneError = ValidationHelper.CheckNullOrWhiteSpace(request.Phone, nameof(request.Phone), fields);
        if (phoneError != null) errorList.Add(phoneError);

        //Logic-based Validations (Roles/Formats)
        if (request.RoleID <= 0)
        {
            errorList.Add(fields.GetValueOrDefault(nameof(request.RoleID), "Invalid Role."));
        }

        // Validate format ONLY if the field wasn't already flagged as missing
        if (emailError == null)
        {
            var emailCheck = EmailHelper.ValidateEmail(request.Email);
            if (!emailCheck.IsValid) errorList.Add(emailCheck.Message);
        }

        if (passwordError == null)
        {
            var passwordCheck = PasswordHelper.ValidatePassword(request.PasswordHash);
            if (!passwordCheck.IsValid) errorList.Add(passwordCheck.Message);
        }

        // Handle accumulated errors
        if (errorList.Any())
        {
            throw new ArgumentException(string.Join(" | ", errorList));
        }

        try
        {
            // Hash the password
            request.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash);

            // Handles the EmailExists check
            var response = await _userRepository.RegisterUser(request);

            return response;
        }
        catch (Exception ex)
        {

            throw new Exception(ex.Message);
        }
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
    
    /// <summary>
    /// Validates and updates user details by an administrator.
    /// </summary>
    /// <param name="request"> The request DTO containing user ID and updated fields such as
    /// name, phone number, role, and status. </param>
    /// <returns> A response DTO containing the updated user information. </returns>
    /// <exception cref="ArgumentNullException"> Thrown when the request object is null. </exception>
    /// <exception cref="ArgumentException"> Thrown when provided data is invalid (e.g., invalid IDs or missing fields). </exception>

    public async Task<UserUpdateByAdminResponseDto> UpdateUser(UserUpdateByAdminRequestDto request)
    {
        var errorList = new List<string>();
        // Check if the request exists
        if (request == null)
            errorList.Add(ErrorMessages.UserUpdate.UpdateUserRequest);

        // Validate that the UserID is a positive number
        if (request.UserID <= 0)
            errorList.Add(ErrorMessages.UserUpdate.InvalidUserId);

        // Validate that the user's name is provided
        if (string.IsNullOrWhiteSpace(request.Name))
           errorList.Add(ErrorMessages.UserUpdate.NameRequired);

        // Validate that the phone number is provided
        if (string.IsNullOrWhiteSpace(request.Phone))
            errorList.Add(ErrorMessages.UserUpdate.PhoneRequired);
        // Validate that the RoleID is valid
        if (request.RoleID <= 0)        
            errorList.Add(ErrorMessages.UserUpdate.InvalidRoleId);

        if (errorList.Any())
            throw new ArgumentException(string.Join(" | ", errorList));

        // Delegate persistence and data update logic to the repository layer
        return await _userRepository.UpdateUser(request);

    }
}