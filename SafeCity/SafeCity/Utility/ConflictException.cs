namespace SafeCity.Utility
{
    /// <summary>
    /// Exception thrown when a resource conflict is detected (e.g., duplicate entry).
    /// </summary>
    public class ConflictException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ConflictException"/> with the specified message.
        /// </summary>
        public ConflictException(string message) : base(message) { }
    }
}
