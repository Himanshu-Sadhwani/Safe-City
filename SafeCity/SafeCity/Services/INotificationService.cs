using SafeCity.DTOs.FieldReport;

namespace SafeCity.Services
{
    /// <summary>
    /// Defines the contract for sending notifications to supervisors.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Sends a notification to the responsible supervisor when a new field report is submitted.
        /// </summary>
        /// <param name="notification">The notification payload containing report and patrol details.</param>
        Task NotifySupervisorAsync(SupervisorNotificationDto notification);
    }
}
