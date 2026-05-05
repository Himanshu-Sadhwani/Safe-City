using Microsoft.AspNetCore.Mvc;
using SafeCity.DTOs.Response;
using SafeCity.Services.Response;
using SafeCity.Utility;

namespace SafeCity.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
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
                var result = await _service.GetCrisisWithResponseAsync(request);

                return Ok(new
                {
                    success = true,
                    message = ErrorMessages.Crisis.FetchSuccess,
                    data = result
                });
            }
            catch (Exception)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ErrorMessages.Crisis.FetchFailed
                });
            }
        }

    }
}
