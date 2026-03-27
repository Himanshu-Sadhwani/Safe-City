using Microsoft.AspNetCore.Mvc;
using SafeCity.DTOs;

namespace SafeCity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="userService">The user service instance used for authentication operations.</param>

        public UserController(IUserService userService)
        {
            _userService = userService;
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
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var result = await _userService.LoginUser(dto);

            if (result == null)
                return Unauthorized(new { message = "Invalid email or password" });

            return Ok(result);
        }
    }
}
