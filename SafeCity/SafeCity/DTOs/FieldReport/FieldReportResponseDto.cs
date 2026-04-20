namespace SafeCity.DTOs.FieldReport
{
    /// <summary>
    /// Data transfer object representing a field report returned to the caller after creation.
    /// </summary>
    public class FieldReportResponseDto
    {
        public int ReportId { get; set; }
        public int PatrolId { get; set; }
        public string Notes { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
