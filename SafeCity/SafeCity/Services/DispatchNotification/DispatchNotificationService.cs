using Microsoft.AspNetCore.SignalR;
using SafeCity.DTOs;
using SafeCity.Hubs;

namespace SafeCity.Services.DispatchNotification
{
    public class DispatchNotificationService : IDispatchNotificationService
    {
        private readonly IHubContext<DispatchHub> _hubContext;

        public DispatchNotificationService(IHubContext<DispatchHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyUnitAssignedAsync(string unitName, DispatchResponseDto response)
        {
            var groupKey = $"unit_{unitName.Replace(" ", "_")}";
            await _hubContext.Clients
                .Group(groupKey)
                .SendAsync("ReceiveDispatchAssignment", response);
        }
    }
}