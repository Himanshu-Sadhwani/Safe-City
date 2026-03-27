namespace SafeCity.Utility
{
    public static class ErrorMessages
    {
        public static class User
        {
            public const string RequestNull = "Registration request cannot be null.";
            public const string RequiredFields = "Required fields are missing or invalid.";
            public const string EmailExists = "A user with this email already exists.";
            public const string InternalError = "An internal server error occurred.";
            public const string UserNotFound = "User does not exist or is inactive.";
            public const string InvalidCredentials = "Invalid email or password.";

            public static readonly Dictionary<string, string> Field = new()
            {
                { "request", "The entire request object is missing or invalid." },
                { "Name", "Full Name is required and cannot be empty." },
                { "Email", "A valid Email address is required." },
                { "PasswordHash", "Password is required." },
                { "PasswordSalt", "Security salt is missing." },
                { "Phone", "Phone number is required for contact." },
                { "RoleID", "A valid User Role must be selected." }
            };
        }

        public static class Validation
        {
            public const string InvalidEmailFormat = "The email address provided is not in a valid format.";
            public const string WeakPassword = "The password does not meet the security requirements.";
        }

        public static class Database
        {
            public const string SaveFailed = "An error occurred while saving the user to the database.";
        }
    }
}