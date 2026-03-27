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
namespace SafeCity.Services;

public class UserService : IUserService
{
    private readonly SafeCityDbContext _context;
    private readonly IConfiguration _config;
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    /// <param name="context">The database context for accessing user and audit log data.</param>
    /// <param name="config">The application configuration used for JWT settings.</param>
    /// <param name="userRepository">The user repository for handling user data operations.</param>

    public UserService(SafeCityDbContext context, IConfiguration config, IUserRepository userRepository)
    {
        _context = context;
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
        var user = await _context.Users
            .Include(u => u.UserRole)
            .FirstOrDefaultAsync(u =>
                u.Email == dto.Email &&
                u.Status == UserStatus.Active
            );

        if (user == null)
            throw new Exception(ErrorMessages.User.UserNotFound);

        var result = new PasswordHasher<User>()
            .VerifyHashedPassword(user, user.PasswordHash, dto.Password);

        if (result == PasswordVerificationResult.Failed)
            throw new Exception(ErrorMessages.User.InvalidCredentials);

        var accessToken = GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();

        await SaveAuditLog(user.UserID, "Login");

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
    /// Saves an audit log entry for user actions such as login.
    /// </summary>
    /// <param name="userId">The ID of the user performing the action.</param>
    /// <param name="action">The description of the action performed.</param>
    /// <returns>A task representing the asynchronous logging operation.</returns>

    private async Task SaveAuditLog(int userId, string action)
    {
        var audit = new AuditLog
        {
            UserID = userId,
            Action = action,
            Resource = "Auth/Login",
            Timestamp = DateTime.UtcNow
        };

        _context.AuditLogs.Add(audit);
        await _context.SaveChangesAsync();
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
}

