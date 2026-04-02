using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SafeCity.DTOs;
using SafeCity.Services.Auth;

namespace SafeCity.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Authenticates a user using email and password credentials.
        /// </summary>
        /// <param name="dto">The login request data transfer object containing email and password.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> containing authentication tokens when successful,
        /// otherwise an Unauthorized error message.
        /// </returns>
    
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            try 
            {
                var result = await _authService.LoginUser(dto);

                if (result == null)
                return Unauthorized(new { message = "Unauthenticated User" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the login request." , error = ex.Message });
            }
        }
    }
}
