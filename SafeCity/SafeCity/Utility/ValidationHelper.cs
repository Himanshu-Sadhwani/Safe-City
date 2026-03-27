public static class ValidationHelper
{
    public static string? CheckNullOrWhiteSpace(
        string? value,
        string fieldName,
        IDictionary<string, string>? errorMessages)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            // Check if the dictionary exists
            // Try to get the value
            if (errorMessages != null && errorMessages.TryGetValue(fieldName, out var message))
            {
                return message;
            }

            return $"{fieldName} is required.";
        }
        return null;
    }
}