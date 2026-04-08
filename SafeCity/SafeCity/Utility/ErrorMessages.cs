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
            public const string NoUsersFound = "No users found.";
            public static readonly Dictionary<string, string> Field = new()
            {
                { "request", "The entire request object is missing or invalid." },
                { "Name", "Full Name is required and cannot be empty." },
                { "Email", "A valid Email address is required." },
                { "PasswordHash", "Password is required." },
                { "Phone", "Phone number is required for contact." },
                { "RoleID", "A valid User Role must be selected." }
            };
        }

        public static class ForgotPassword
        {
            public const string UserNotFound = "No user found with the provided email address.";
            public const string ProcessingFailed = "An error occurred while processing the forgot password request.";
            public const string SameAsOldPassword = "The new password must be different from the old password.";
        }
        public static class UserUpdate
        {
            public const string UpdateUserRequest = "Update request cannot be null.";
            public const string InvalidUserId = "Invalid UserID.";
            public const string UserNotFound = "User not found.";
            public const string InvalidStatus = "Invalid user status.";
            public const string NameRequired = "Name is required.";
            public const string PhoneRequired = "Phone number is required.";
            public const string InvalidRoleId = "Invalid RoleID.";
            public const string UpdateFailed = "User update failed.";
            public const string InvalidPhoneNo = "Invalid Phone Number";
        }


        public static class Validation
        {
            public const string InvalidEmailFormat = "The email address provided is not in a valid format.";
            public const string WeakPassword = "The password does not meet the security requirements.";
        }

        public static class Database
        {
            public const string SaveFailed = "An error occurred while saving the user to the database.";
            public const string UpdateFailed = "An error occurred while updating the user in the database.";
            public const string ForgotPasswordFailed = "An error occured while processing the passwrod request.";
        }

        public static class Audit
        {
            public const string SaveFailed = "An error occurred while saving the audit record.";
            public const string RequestNull = "Audit request cannot be null.";
            public const string InvalidOfficerID = "Officer ID must be a valid positive number.";
            public const string FindingsRequired = "Findings are required and cannot be empty.";
            public const string InternalError = "An internal server error occurred.";
            public const string InvalidScope = "The provided audit scope is not valid.";
            public const string InvalidStatus = "The provided audit status is not valid.";
            public const string OfficerNotFound = "The specified officer does not exist or does not have a valid officer role.";
        }
    }
}