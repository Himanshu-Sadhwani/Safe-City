using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeCity.DTOs;
using SafeCity.Services.Dispatch;
using SafeCity.Services.DispatchNotification;

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
        private readonly IDispatchNotificationService _notificationService;
        private readonly ILogger<DispatchController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DispatchController"/> class.
        /// </summary>
        /// <param name="dispatchService">
        /// The dispatch service that handles business logic for unit assignment.
        /// </param>
        /// <param name="notificationService">
        /// The notification service that handles real-time SignalR dispatch alerts.
        /// </param>
        public DispatchController(
            IDispatchService dispatchService,
            IDispatchNotificationService notificationService,
            ILogger<DispatchController> logger)
        {
            _dispatchService = dispatchService;
            _notificationService = notificationService;
            _logger = logger;
        }

        /// <summary>
        /// Assigns an available resource unit to an incident.
        /// </summary>
        /// <param name="request">
        /// The dispatch request containing incident and dispatcher information.
        /// </param>
        /// <returns>
        /// Returns <see cref="OkObjectResult"/> with dispatch details if successful,
        /// or <see cref="BadRequestObjectResult"/> if validation or processing fails.
        /// </returns>
        /// <response code="200">Resource successfully assigned to the incident.</response>
        /// <response code="400">Invalid request data or assignment failure.</response>
        [Authorize(Roles = "Emergency_Dispatcher , Admin")]
        [HttpPost("assign")]
        public async Task<IActionResult> AssignUnit([FromBody] DispatchRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            DispatchResponseDto? result = null;

            // Step 1: Dispatch business logic
            try
            {
                result = await _dispatchService.AssignUnitAsync(request);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

            // Step 2: Send real-time notification to assigned unit
            try
            {
                await _notificationService.NotifyUnitAssignedAsync(result.UnitName, result);
            }
            catch (Exception notifyEx)
            {
                // Dispatch succeeded — do not return 400.
                // Log the notification failure and return 200 with a warning.
                _logger.LogError(notifyEx, "SignalR notification failed for unit {UnitName}", result.UnitName);
                return Ok(new
                {
                    message = "Successfully Dispatched Resource (notification failed)",
                    notificationError = notifyEx.Message
                });
            }

            return Ok(new { message = "Successfully Dispatched Resource" });
        }
        
        /// <summary>
        /// Updates the status of an existing dispatch.
        /// </summary>
        /// <param name="id">The unique identifier of the dispatch.</param>
        /// <param name="request">Request containing the updated dispatch status.</param>
        /// <returns>
        /// Returns <see cref="OkObjectResult"/> if the update is successful,
        /// or <see cref="BadRequestObjectResult"/> if the update fails.
        /// </returns>
        /// <response code="200">Dispatch status updated successfully.</response>
        /// <response code="400">Invalid request data or update failure.</response>
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
    }
}