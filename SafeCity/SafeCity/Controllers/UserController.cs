using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        /// Retrieves a specific user's details based on the provided unique user ID.
        /// </summary>
        /// <param name="id">The unique identifier of the user whose information is being requested.</param>
        /// <returns>
        /// An IActionResult containing the user's details if found, 
        /// or an appropriate error message if the user does not exist or an error occurs.
        /// </returns>
        
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ViewOneUserResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var result = await _userService.GetUserByIdAsync(id);

                if (result == null)
                    return NotFound(ErrorMessages.User.UserNotFound);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ErrorMessages.User.InternalError}: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a complete list of all registered users in the SafeCity system.
        /// </summary>
        /// <returns>
        /// An IActionResult containing a list of user details if users exist, 
        /// or an appropriate error message if no users are found or an unexpected error occurs.
        /// </returns>
        
        [HttpGet]
        [ProducesResponseType(typeof(List<ViewAllUsersResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var result = await _userService.GetAllUsersAsync();

                if (result == null || !result.Any())
                    return NotFound(ErrorMessages.User.NoUsersFound);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ErrorMessages.User.InternalError}: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Updates user details by an administrator.
        /// </summary>
        /// <param name="user">User details to be updated by admin</param>
        /// <returns>Returns updated user information</returns>
        /// <response code="200">User updated successfully</response>
        /// <response code="400">Invalid request or validation error</response>
        /// <response code="500">Server error</response> 
        
        [Authorize(Roles = "City_Administrator")]  
        [HttpPut("update/{id}")]  
        [ProducesResponseType(typeof(UserUpdateByAdminResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUserByAdmin(
            [FromRoute] int id,
            [FromBody] UserUpdateByAdminRequestDto user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
 
                var response = await _userService.UpdateUser(id,user);
                return Ok(new {
                    message = "Successfully updated",
                    data = response
                });
            }
           
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new {
                    error = ErrorMessages.Database.UpdateFailed
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new {
                    error = ErrorMessages.User.InternalError
                });
            }
        }

        /// <summary>
        /// Initiates the forgot password process for a user.
        /// </summary>
        /// <param name="request">The forgot password request DTO containing the registered email or username.</param>
        /// <returns>Returns a response indicating whether the password reset process was initiated successfully.</returns>
        /// <response code="200">Forgot password request processed successfully</response>
        /// <response code="400">Invalid request or required fields are missing</response>
        /// <response code="500">Server error while processing the forgot password request</response>
        [HttpPut("forgotpassword")]
        [ProducesResponseType(typeof(ForgotPasswordResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ErrorMessages.User.RequiredFields);
                }

                var response = await _userService.ForgotPassword(request);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, ErrorMessages.Database.ForgotPasswordFailed);
            }
        }
    }

    }
