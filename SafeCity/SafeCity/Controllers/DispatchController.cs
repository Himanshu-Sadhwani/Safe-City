using Microsoft.AspNetCore.Mvc;
using SafeCity.DTOs;
using SafeCity.Services;
using SafeCity.Services.Dispatch;

namespace SafeCity.Controllers
{
    [ApiController]
    [Route("api/dispatch")]
    public class DispatchController : ControllerBase
    {
        private readonly IDispatchService _dispatchService;

        public DispatchController(IDispatchService dispatchService)
        {
            _dispatchService = dispatchService;
        }

        // POST: api/dispatch/assign
        [HttpPost("assign")]
        public async Task<IActionResult> AssignUnit([FromBody] DispatchRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _dispatchService.AssignUnitAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
