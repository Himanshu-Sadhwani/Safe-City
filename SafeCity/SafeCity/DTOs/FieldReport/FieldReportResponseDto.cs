namespace SafeCity.DTOs.FieldReport
{
    /// <summary>
    /// Data transfer object representing a field report returned to the caller after creation.
    /// </summary>
    public class FieldReportResponseDto
    {
        /// <summary>Gets or sets the unique identifier of the field report.</summary>
        public int ReportId { get; set; }

        /// <summary>Gets or sets the patrol identifier associated with this report.</summary>
        public int PatrolId { get; set; }

        /// <summary>Gets or sets the field activity notes.</summary>
        public string Notes { get; set; } = string.Empty;

        /// <summary>Gets or sets the date the field activity occurred.</summary>
        public DateTime Date { get; set; }

        /// <summary>Gets or sets the current status of the report as a human-readable string.</summary>
        public string Status { get; set; } = string.Empty;
    }
}
