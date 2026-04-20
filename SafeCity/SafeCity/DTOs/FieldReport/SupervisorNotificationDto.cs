namespace SafeCity.DTOs.FieldReport
{
    /// <summary>
    /// Data transfer object carrying the notification payload sent to a supervisor
    /// when a new field report is created.
    /// </summary>
    public class SupervisorNotificationDto
    {
        /// <summary>Gets or sets the unique identifier of the created field report.</summary>
        public int ReportId { get; set; }

        /// <summary>Gets or sets the patrol identifier the report is linked to.</summary>
        public int PatrolId { get; set; }

        /// <summary>Gets or sets the UTC timestamp when the event occurred.</summary>
        public DateTime OccurredAt { get; set; }

        /// <summary>Gets or sets the human-readable notification message.</summary>
        public string Message { get; set; } = string.Empty;
    }
}
