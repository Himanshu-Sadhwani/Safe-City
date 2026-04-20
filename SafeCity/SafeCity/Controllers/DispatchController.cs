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
            IDispatchNotificationService notificationService)
        {
            _dispatchService = dispatchService;
            _notificationService = notificationService;
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
                Console.WriteLine($"[SignalR ERROR] {notifyEx.Message}");
                return Ok(new
                {
                    message = "Successfully Dispatched Resource (notification failed)",
                    notificationError = notifyEx.Message
                });
            }

            return Ok(new { message = "Successfully Dispatched Resource" });
        }
    }
}