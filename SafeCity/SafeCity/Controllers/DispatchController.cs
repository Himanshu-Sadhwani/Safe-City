using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeCity.Domain.Enum;
using SafeCity.DTOs;
using SafeCity.Services.Dispatch;

namespace SafeCity.Controllers
{
    /// <summary>
    /// API controller responsible for dispatch-related operations.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DispatchController : ControllerBase
    {
        private readonly IDispatchService _dispatchService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DispatchController"/> class.
        /// </summary>
        /// <param name="dispatchService">
        /// The dispatch service that handles business logic for unit assignment.
        /// </param>
        public DispatchController(IDispatchService dispatchService)
        {
            _dispatchService = dispatchService;
        }

        /// <summary>
        /// Assigns an available resource unit to an incident. </summary>
        /// <param name="request">
        /// The dispatch request containing incident and dispatcher information.</param>
        /// <returns>
        /// Returns <see cref="OkObjectResult"/> with dispatch details if successful,
        /// or <see cref="BadRequestObjectResult"/> if validation or processing fails.</returns>
        /// <response code="200">Resource successfully assigned to the incident.</response>
        /// <response code="400">Invalid request data or assignment failure.</response>
        [Authorize(Roles = "Emergency_Dispatcher , Admin")]
        [HttpPost("assign")]
        public async Task<IActionResult> AssignUnit([FromBody] DispatchRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var DispatcherID=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                int DispatcherId=int.Parse(DispatcherID);
                var result = await _dispatchService.AssignUnitAsync(DispatcherId,request);
                return Ok(new
                {
                    message = "Successfully Dispatched Resource",
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
         // <summary>
        /// Updates the real-time status of a dispatched unit.
        /// </summary>
        /// <param name="request">Dispatch status update request.</param>
        [Authorize(Roles = "Emergency_Dispatcher , Admin")]
        [HttpPatch("status/{id}")]
        public async Task<IActionResult> UpdateStatus(
           [FromRoute] int id, [FromBody] DispatchUpdateByStatusRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _dispatchService.UpdateDispatchStatusAsync(id,request);
                return Ok(new { message = "Dispatch status updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Emergency_Dispatcher , Admin")]
        [HttpGet("list")]
        public async Task<IActionResult> ViewDispatch(
            [FromQuery] int? incidentId,
            [FromQuery] int? resourceId,
            [FromQuery] int? dispatcherId,
            [FromQuery] DispatchStatusOption? status,
            [FromQuery] DateTime? date,
            [FromQuery] string? sortOrder)
        {
            try
            {
                // Extracting user information from token
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                bool isAdmin = User.IsInRole("Admin");

                // If not admin, force dispatcherId from token
                if (!isAdmin)
                {
                    dispatcherId = userId;
                }

                // Calling service layer
                var response = await _dispatchService.ViewDispatch(
                    incidentId,
                    isAdmin,
                    resourceId,
                    dispatcherId,
                    status,
                    date,
                    sortOrder
                );

                if (response == null || response.Count == 0)
                {
                    return NotFound("No dispatch records found.");
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                // throws errors if any present while handling the request
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}