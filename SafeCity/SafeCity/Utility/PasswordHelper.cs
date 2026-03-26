namespace SafeCity.Utility
{
    public static class PasswordHelper
    {
        public static (bool IsValid, string Message) ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return (false, "Password cannot be empty.");

            if (password.Length < 8)
                return (false, "Minimum 8 characters required.");

            if (!password.Any(char.IsUpper))
                return (false, "At least one uppercase letter required.");

            if (!password.Any(char.IsLower))
                return (false, "At least one lowercase letter required.");

            if (!password.Any(char.IsDigit))
                return (false, "At least one numeric digit required.");
            if (!password.Any(char.IsPunctuation))
                return (false, "At least one Special character is required.");
            return (true, "Success");
        }
    }
}
