namespace SafeCity.DTOs.FieldReport
{
    /// <summary>
    /// Data transfer object carrying the notification payload sent to a supervisor
    /// when a new field report is created.
    /// </summary>
    public class SupervisorNotificationDto
    {
        public int ReportId { get; set; }
        public int PatrolId { get; set; }
        public DateTime OccurredAt { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
