using SafeCity.DTOs.FieldReport;

namespace SafeCity.Services
{
    /// <summary>
    /// Temporary no-op implementation of <see cref="INotificationService"/> for development and testing.
    /// Replace with a real implementation (email, SignalR, etc.) before production.
    /// </summary>
    public class StubNotificationService : INotificationService
    {
        /// <summary>
        /// Logs the notification to the console. Does not send any real notification.
        /// </summary>
        public Task NotifySupervisorAsync(SupervisorNotificationDto notification)
        {
            Console.WriteLine($"[STUB] Supervisor notified — ReportId: {notification.ReportId}, PatrolId: {notification.PatrolId}, Message: {notification.Message}");
            return Task.CompletedTask;
        }
    }
}
