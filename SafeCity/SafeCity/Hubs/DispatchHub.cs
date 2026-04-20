using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace SafeCity.Hubs
{

    public class DispatchHub : Hub
    {
        /// <summary>
        /// Called by responder clients to subscribe to their unit's dispatch notifications.
        /// </summary>
        public async Task JoinUnitGroup(string unitName)
        {
            var groupKey = $"unit_{unitName.Replace(" ", "_")}";
            await Groups.AddToGroupAsync(Context.ConnectionId, groupKey);
        }

        public async Task LeaveUnitGroup(string unitName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"unit_{unitName}");
        }
    }
}