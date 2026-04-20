namespace SafeCity.Utility
{
    /// <summary>
    /// Exception thrown when a requested resource cannot be found.
    /// </summary>
    public class NotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="NotFoundException"/> with the specified message.
        /// </summary>
        public NotFoundException(string message) : base(message) { }
    }
}
