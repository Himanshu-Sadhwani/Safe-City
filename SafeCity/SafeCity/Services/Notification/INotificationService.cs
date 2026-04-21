using SafeCity.DTOs.Notification;

namespace SafeCity.Services.Notification
{
    public interface INotificationService
    {
        /// <summary>
        /// Sends a real-time notification to all clients in the specified group.
        /// </summary>
        /// <param name="notification">The notification containing group, event, and payload.</param>
        Task SendAsync(NotificationDto notification);
    }
}