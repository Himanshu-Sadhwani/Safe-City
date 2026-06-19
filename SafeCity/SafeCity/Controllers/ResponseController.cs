using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeCity.DTOs.Response;
using SafeCity.Services.Response;
using SafeCity.Utility;

namespace SafeCity.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize(Roles = nameof(UserRoleOption.Admin) + "," + nameof(UserRoleOption.Police))]
    public class ResponseController : ControllerBase
    {
        private readonly IResponseService _service;

        public ResponseController(IResponseService service)
        {
            _service = service;
        }

        [HttpPost("assign-team")]
        public async Task<IActionResult> AssignResponseTeam([FromBody] AssignResponseTeamRequestDto dto)
        {
            try
            {
                var result = await _service.AssignResponseTeamAsync(dto);
                return Ok(new
                {
                    success = true,
                    message = "Response team assigned successfully...."
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
        [HttpGet("crisis-response")]
        public async Task<IActionResult> GetCrisisResponse([FromQuery] GetCrisisResponseRequestDto request)
        {
            try
            {
                var result =
                    await _service
                    .GetCrisisWithResponseAsync(request);

                if (result == null || !result.Any())
                {
                    string message = ErrorMessages.Response.NoData;

                    if (!string.IsNullOrWhiteSpace(request.Location))
                    {
                        message = ErrorMessages.Response.NoDataByLocation;
                    }
                    else if (request.Status.HasValue)
                    {
                        message = ErrorMessages.Response.NoDataByStatus;
                    }
                    else if (request.Severity.HasValue)
                    {
                        message = ErrorMessages.Response.NoDataBySeverity;
                    }
                    else if (request.TeamId.HasValue)
                    {
                        message = ErrorMessages.Response.NoDataByTeam;
                    }
                    else if (request.CrisisId.HasValue)
                    {
                        message = ErrorMessages.Response.NoDataByCrisisId;
                    }

                    return Ok(new
                    {
                        success = false,
                        message = message,
                        data = new List<object>()
                    });
                }

                // Success Response
                return Ok(new
                {
                    message = ErrorMessages.Response.FetchSuccess,
                    data = result
                });
            }

            // Validation Error
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }

            // Internal Server Error
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ErrorMessages.Response.FetchFailed
                });
            }
        }
    }
}
