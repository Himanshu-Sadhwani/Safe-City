using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeCity.DTOs;
using SafeCity.Services;
using SafeCity.Utility;
namespace SafeCity.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Registers a new user into the SafeCity system.
        /// </summary>
        /// <param name="user">The user registration data transfer object containing credentials and profile info.</param>
        /// <returns>An IActionResult containing the registration response or an error message.</returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(UserRegisterResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegisterUser(UserRegisterRequestDto user)
        {
            try
            {
                // Validate Model State
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = await _userService.RegisterUser(user);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
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
        /// <summary>
        /// Updates user details by an administrator.
        /// </summary>
        /// <param name="user">User details to be updated by admin</param>
        /// <returns>Returns updated user information</returns>
        /// <response code="200">User updated successfully</response>
        /// <response code="400">Invalid request or validation error</response>
        /// <response code="500">Server error</response> 
        [Authorize(Roles = "Admin")]  
        [HttpPut("update")]   
        [ProducesResponseType(typeof(UserUpdateByAdminResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUserByAdmin(
            [FromBody] UserUpdateByAdminRequestDto user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var response = await _userService.UpdateUser(user);
                return Ok(response);
            }
            catch (ArgumentNullException)
            {
                return BadRequest(new
                {
                    error = ErrorMessages.UserUpdate.UpdateUserRequest
                });
            }

            catch (Exception ex)
            {
                //Throwing Exception
                return StatusCode(StatusCodes.Status500InternalServerError,ErrorMessages.User.InternalError);
            } 
        }
    }
}
