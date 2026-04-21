using MediatR;
using SafeCity.DTOs.FieldReport;
using SafeCity.Services;

namespace SafeCity.Events.FieldReport
{
    /// <summary>
    /// Handles the <see cref="FieldReportCreatedEvent"/> by raising an alert that a field report was created.
    /// </summary>
    public class FieldReportCreatedEventHandler : INotificationHandler<FieldReportCreatedEvent>
    {
        private readonly INotificationService _notificationService;

        public FieldReportCreatedEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Handle(FieldReportCreatedEvent notification, CancellationToken cancellationToken)
        {
            var dto = new SupervisorNotificationDto
            {
                ReportId   = notification.ReportId,
                PatrolId   = notification.PatrolId,
                OccurredAt = DateTime.UtcNow,
                Message    = "Field report created"
            };

            await _notificationService.NotifySupervisorAsync(dto);
        }
    }
}
