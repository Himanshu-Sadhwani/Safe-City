using Microsoft.AspNetCore.SignalR;
using SafeCity.DTOs.Notification;
using SafeCity.Hubs;

namespace SafeCity.Services.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendAsync(NotificationDto notification)
        {
            await _hubContext.Clients
                .Group(notification.TargetGroup)
                .SendAsync(notification.Event, notification.Payload);
        }
    }
}