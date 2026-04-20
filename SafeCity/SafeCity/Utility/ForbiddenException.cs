namespace SafeCity.Utility
{
    /// <summary>
    /// Exception thrown when an authenticated user attempts an action they are not permitted to perform.
    /// </summary>
    public class ForbiddenException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ForbiddenException"/> with the specified message.
        /// </summary>
        public ForbiddenException(string message) : base(message) { }
    }
}
