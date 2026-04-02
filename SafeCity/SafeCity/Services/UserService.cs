using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SafeCity.Domain.Entity;
using SafeCity.DTOs;
using SafeCity.Repository;
using SafeCity.Domain.Entity;
using SafeCity.Domain.Data;
using Microsoft.EntityFrameworkCore;
using SafeCity.Utility;
using SafeCity.Domain.Enum;
using Microsoft.AspNetCore.Identity.Data;
namespace SafeCity.Services;

public class UserService : IUserService
{
    private readonly IConfiguration _config;
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    /// <param name="config">The application configuration used for JWT settings.</param>
    /// <param name="userRepository">The user repository for handling user data operations.</param>
    public UserService(IConfiguration config, IUserRepository userRepository)
    {
        _config = config;
        _userRepository = userRepository;
    }

    /// <summary>
    /// Authenticates a user by validating credentials and generating auth tokens.
    /// </summary>
    /// <param name="dto">The login request DTO containing email and password.</param>
    /// <returns>
    /// A <see cref="LoginResponseDto"/> containing the access token, refresh token, 
    /// and expiration details if successful; otherwise, null.
    /// </returns>
    public async Task<LoginResponseDto> LoginUser(LoginRequestDto dto)
    {
        var user = await _userRepository.GetUserByEmailAndStatusAsync(
            dto.Email,
            UserStatus.Active
        );

        if (user == null)
            throw new Exception(ErrorMessages.User.UserNotFound);

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
        if (!isPasswordValid)
            throw new Exception(ErrorMessages.User.InvalidCredentials);

        var accessToken = GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();

        await _userRepository.SaveAuditLogAsync(user.UserID, "Login");

        return new LoginResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Expires = DateTime.UtcNow.AddHours(1)
        };
    }

    /// <summary>
    /// Generates a JWT access token for the authenticated user.
    /// </summary>
    /// <param name="user">The user entity for whom the token is being created.</param>
    /// <returns>A signed JWT token string containing user claims.</returns>
    private string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.UserRole.RoleName.ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"])
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// Generates a secure random refresh token for long-lived authentication.
    /// </summary>
    /// <returns>A Base64 encoded secure refresh token string.</returns>
    private string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

    /// <summary>
    /// Checks the user's data, hashes the password, and saves the user to the database.
    /// </summary>
    /// <param name="request">The data provided for registration.</param>
    /// <returns>The result of the registration process.</returns>
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
    public async Task<ViewOneUserResponseDto> GetUserByIdAsync(int userId)
    {
        try
        {
            // Fetch user entity from repository 
            var user = await _userRepository.GetUserByIdAsync(userId);
            // Return null if the user ID does not exist in the database
            if (user == null)
            {
                Console.WriteLine(ErrorMessages.User.UserNotFound);
                return null;
            }
            // Map database entity to response DTO for client consumption
            return new ViewOneUserResponseDto
            {
                UserId = user.UserID,
                UserName = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                Status = user.Status.ToString(),
                RoleName = user.UserRole.RoleName.ToString()
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ErrorMessages.User.InternalError} - {ex.Message}");
            return null;
        }
    }
    public async Task<List<ViewAllUsersResponseDto>> GetAllUsersAsync()
    {
        try
        {
            // Retrieve all users including associated roles
            var users = await _userRepository.GetAllUsersAsync();

            // Handle case where no users exist in the database
            if (users == null || users.Count == 0)
            {
                Console.WriteLine(ErrorMessages.User.NoUsersFound);
                return new List<ViewAllUsersResponseDto>();
            }

            // Convert user entities into response DTO list
            return users.Select(user => new ViewAllUsersResponseDto
            {
                UserId = user.UserID,
                UserName = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                Status = user.Status.ToString(),
                RoleName = user.UserRole.RoleName.ToString()

            }).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ErrorMessages.User.InternalError} - {ex.Message}");
            return new List<ViewAllUsersResponseDto>();
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
            throw new ArgumentNullException(nameof(request), ErrorMessages.UserUpdate.UpdateUserRequest);

        // Validate that the UserID is a positive number
        if (request.UserID <= 0)
            throw new ArgumentNullException(nameof(request), ErrorMessages.UserUpdate.InvalidUserId);

        // Validate that the user's name is provided
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentNullException(nameof(request), ErrorMessages.UserUpdate.NameRequired);

        // Validate that the phone number is provided
        if (string.IsNullOrWhiteSpace(request.Phone))
            throw new ArgumentNullException(nameof(request), ErrorMessages.UserUpdate.PhoneRequired);

        // Validate that the RoleID is valid
        if (request.RoleID <= 0)
            throw new ArgumentNullException(nameof(request), ErrorMessages.UserUpdate.InvalidRoleId); ;

        // Delegate persistence and data update logic to the repository layer
        return await _userRepository.UpdateUser(request);

    }
}
