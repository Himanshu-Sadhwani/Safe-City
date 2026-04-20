using MediatR;

namespace SafeCity.Events
{
    /// <summary>
    /// Domain event raised when a field report is successfully created and persisted.
    /// Handlers of this event are responsible for downstream actions such as supervisor notifications.
    /// </summary>
    public class FieldReportCreatedEvent : INotification
    {
        /// <summary>Gets the unique identifier of the newly created field report.</summary>
        public int ReportId { get; }

        /// <summary>Gets the patrol identifier associated with the field report.</summary>
        public int PatrolId { get; }

        /// <summary>Gets the date recorded on the field report.</summary>
        public DateTime Date { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="FieldReportCreatedEvent"/>.
        /// </summary>
        /// <param name="reportId">The ID of the created report.</param>
        /// <param name="patrolId">The ID of the associated patrol.</param>
        /// <param name="date">The date of the field activity.</param>
        public FieldReportCreatedEvent(int reportId, int patrolId, DateTime date)
        {
            ReportId = reportId;
            PatrolId = patrolId;
            Date = date;
        }
    }
}
