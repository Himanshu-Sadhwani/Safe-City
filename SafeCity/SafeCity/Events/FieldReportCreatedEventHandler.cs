using MediatR;
using SafeCity.DTOs.FieldReport;
using SafeCity.Services;

namespace SafeCity.Events
{
    /// <summary>
    /// Handles the <see cref="FieldReportCreatedEvent"/> by forwarding a notification to the supervisor
    /// via <see cref="INotificationService"/>.
    /// </summary>
    public class FieldReportCreatedEventHandler : INotificationHandler<FieldReportCreatedEvent>
    {
        private readonly INotificationService _notificationService;

        /// <summary>
        /// Initializes a new instance of <see cref="FieldReportCreatedEventHandler"/>.
        /// </summary>
        public FieldReportCreatedEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        /// <summary>
        /// Builds a <see cref="SupervisorNotificationDto"/> and delegates delivery to
        /// <see cref="INotificationService.NotifySupervisorAsync"/>.
        /// </summary>
        public async Task Handle(FieldReportCreatedEvent notification, CancellationToken cancellationToken)
        {
            var dto = new SupervisorNotificationDto
            {
                ReportId   = notification.ReportId,
                PatrolId   = notification.PatrolId,
                OccurredAt = DateTime.UtcNow,
                Message    = $"Field report #{notification.ReportId} has been submitted for patrol {notification.PatrolId}."
            };

            await _notificationService.NotifySupervisorAsync(dto);
        }
    }
}
