using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeCity.DTOs.Patrol;
using SafeCity.Services.PatrolService;

namespace SafeCity.Controllers
{
    [Route("api/v1/patrols")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class PatrolController : ControllerBase
    {
        private readonly IPatrolService _patrolService;

        public PatrolController(IPatrolService patrolService)
        {
            _patrolService = patrolService;
        }

        [HttpGet("available-officers")]
        public async Task<IActionResult> GetAvailableOfficers([FromQuery] DateTime date)
        {
            try
            {
                var officers = await _patrolService.GetAvailableOfficersAsync(date);
                return Ok(officers);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePatrol([FromBody] CreatePatrolRequestDto requestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var response = await _patrolService.CreatePatrolAsync(requestDto);
                return CreatedAtAction(nameof(CreatePatrol), new { id = response.PatrolId }, response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
