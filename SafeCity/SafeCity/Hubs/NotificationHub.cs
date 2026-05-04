using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace SafeCity.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        /// <summary>
        /// Called by clients to subscribe to a specific group.
        /// e.g. "unit_Fire Unit AN-03", "patrol_P01", "crisis_zone_1"
        /// </summary>
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            // Send confirmation back to the caller
            await Clients.Caller.SendAsync("GroupJoined", groupName);
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}