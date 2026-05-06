using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeCity.DTOs.Notification;
using SafeCity.Services.Notification;

namespace SafeCity.Controllers
{
    /// <summary>
    /// Controller responsible for managing real-time notification group subscriptions.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(
            INotificationService notificationService,
            ILogger<NotificationController> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        /// <summary>
        /// Manually sends a notification to a target group. 
        /// Useful for admin broadcasts or crisis alerts.
        /// </summary>
        [Authorize(Roles = nameof(UserRoleOption.Admin))]
        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] NotificationDto notification)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _notificationService.SendAsync(notification);
                return Ok(new { message = $"Notification sent to group '{notification.TargetGroup}'" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send notification to group {Group}", notification.TargetGroup);
                return StatusCode(500, new { message = "Failed to send notification" });
            }
        }
    }
}