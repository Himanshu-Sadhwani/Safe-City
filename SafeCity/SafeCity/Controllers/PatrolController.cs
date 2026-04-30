using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeCity.DTOs.Patrol;
using SafeCity.Services.PatrolService;

namespace SafeCity.Controllers
{
    /// <summary>
    /// Handles patrol management operations.
    /// All endpoints require Admin authorization.
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(UserRoleOption.Admin))]
    public class PatrolController : ControllerBase
    {
        private readonly IPatrolService _patrolService;

        /// <summary>
        /// Initializes the PatrolController with the patrol service.
        /// </summary>
        /// <param name="patrolService">Service handling patrol business logic.</param>
        public PatrolController(IPatrolService patrolService)
        {
            _patrolService = patrolService;
        }

        /// <summary>
        /// Retrieves a list of police officers available for patrol on the given date.
        /// </summary>
        /// <param name="date">The date to check officer availability.</param>
        /// <returns>List of available officers, or an error if none found or date is invalid.</returns>
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

        /// <summary>
        /// Retrieves all patrols with optional filters for PatrolId, OfficerId, and Date.
        /// </summary>
        /// <param name="filter">Optional query filters.</param>
        /// <returns>List of patrols matching the filters.</returns>
        [HttpGet]
        [Authorize(Roles = nameof(UserRoleOption.Admin))]
        public async Task<IActionResult> GetAllPatrols([FromQuery] PatrolFilterDto filter)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _patrolService.GetAllAsync(filter);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
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
