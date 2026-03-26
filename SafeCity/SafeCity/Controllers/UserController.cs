using Microsoft.AspNetCore.Mvc;
using SafeCity.DTOs;
using SafeCity.Services;
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
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("register")]
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
                return StatusCode(500, $"Internal server error: {ex.Message}");
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
        [HttpPut("admin/update")]
        public async Task<IActionResult> UpdateUserByAdmin(
            [FromBody] UserUpdateByAdminRequestDto user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var response = await _userService.UpdateUserByAdmin(user);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500,$"Internal server error: {ex.Message}");
            }
        }
    }
}
