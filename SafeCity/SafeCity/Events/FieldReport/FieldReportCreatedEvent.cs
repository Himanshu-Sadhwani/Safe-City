using MediatR;

namespace SafeCity.Events.FieldReport
{
    /// <summary>
    /// Domain event raised when a field report is successfully created and persisted.
    /// Handlers of this event are responsible for downstream actions such as supervisor notifications.
    /// </summary>
    public class FieldReportCreatedEvent : INotification
    {
        public int ReportId { get; }
        public int PatrolId { get; }
        public DateTime Date { get; }

        public FieldReportCreatedEvent(int reportId, int patrolId, DateTime date)
        {
            ReportId = reportId;
            PatrolId = patrolId;
            Date = date;
        }
    }
}
