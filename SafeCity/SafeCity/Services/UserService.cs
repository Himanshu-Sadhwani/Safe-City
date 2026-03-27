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
namespace SafeCity.Services;

public class UserService : IUserService
{
    private readonly SafeCityDbContext _context;
    private readonly IConfiguration _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    /// <param name="context">The database context for accessing user and audit log data.</param>
    /// <param name="config">The application configuration used for JWT settings.</param>

    public UserService(SafeCityDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    /// <summary>
    /// Authenticates a user by validating credentials and generating auth tokens.
    /// </summary>
    /// <param name="dto">The login request DTO containing email and password.</param>
    /// <returns>
    /// A <see cref="LoginResponseDto"/> containing the access token, refresh token, 
    /// and expiration details if successful; otherwise, null.
    /// </returns>

    public async Task<LoginResponseDto?> LoginUser(LoginRequestDto dto)
    {
        var user = await _context.Users
            .Include(u => u.UserRole)
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user == null)
            return null;

        var result = new PasswordHasher<User>()
            .VerifyHashedPassword(user, user.PasswordHash, dto.Password);

        if (result == PasswordVerificationResult.Failed)
            return null;

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

    public string GenerateJwtToken(User user)
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

    public string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

    /// <summary>
    /// Saves an audit log entry for user actions such as login.
    /// </summary>
    /// <param name="userId">The ID of the user performing the action.</param>
    /// <param name="action">The description of the action performed.</param>
    /// <returns>A task representing the asynchronous logging operation.</returns>

    public async Task SaveAuditLog(int userId, string action)
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
}
