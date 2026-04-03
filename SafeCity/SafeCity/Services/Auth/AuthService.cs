using Microsoft.IdentityModel.Tokens;
using SafeCity.Domain.Entity;
using SafeCity.Domain.Enum;
using SafeCity.DTOs;
using SafeCity.Repository;
using SafeCity.Utility;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SafeCity.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IConfiguration _config;
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    /// <param name="config">The application configuration used for JWT settings.</param>
    /// <param name="userRepository">The user repository for handling user data operations.</param>
    public AuthService(IConfiguration config, IUserRepository userRepository)
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
        if (string.IsNullOrWhiteSpace(dto.Email))
            throw new ArgumentException(ErrorMessages.Login.EmailRequired);

        if (string.IsNullOrWhiteSpace(dto.Password))
            throw new ArgumentException(ErrorMessages.Login.PasswordRequired);

        var emailRegex = new System.Text.RegularExpressions.Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        if (!emailRegex.IsMatch(dto.Email))
            throw new ArgumentException(ErrorMessages.Validation.InvalidEmailFormat);

        var user = await _userRepository.GetUserByEmailAndStatusAsync(
            dto.Email,
            UserStatus.Active
        );

        if (user == null)
            throw new UnauthorizedAccessException(ErrorMessages.User.InvalidCredentials);

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);
        if (!isPasswordValid)
            throw new UnauthorizedAccessException(ErrorMessages.User.InvalidCredentials);

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
}
