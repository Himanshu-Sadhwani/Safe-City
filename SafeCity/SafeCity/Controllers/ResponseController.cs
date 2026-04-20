using Microsoft.AspNetCore.Mvc;
using SafeCity.DTOs.Response;
using SafeCity.Services.Response;

namespace SafeCity.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
                    data = result
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
    }
}
