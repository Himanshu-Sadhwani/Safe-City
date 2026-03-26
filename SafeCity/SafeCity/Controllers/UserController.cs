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
        /// Updates user details by an administrator.
        /// </summary>
        /// <param name="user">User details to be updated by admin</param>
        /// <returns>Returns updated user information</returns>
        /// <response code="200">User updated successfully</response>
        /// <response code="400">Invalid request or validation error</response>
        /// <response code="500">Server error</response>
        [HttpPut("admin/update")]       
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

                var response = await _userService.UpdateUserByAdmin(user);
                return Ok(response);
            }
            catch (ArgumentNullException)
            {
                return BadRequest(new
                {
                    error = ErrorMessageUpdate.UserUpdate.RequestNull
                });
            }

            catch (Exception ex)
            {
                //Throwing Exception
                return StatusCode(StatusCodes.Status500InternalServerError,ErrorMessageUpdate.User.InternalError);
            }
            
        }
    }
}
