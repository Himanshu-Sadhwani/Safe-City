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
            public const string UserNotFound = "User does not exist.";
            public const string InvalidCredentials = "Invalid email or password.";
            public const string NoUsersFound = "No users found.";
            public const string InvalidPassword = "Invalid password.";
            public static readonly Dictionary<string, string> Field = new()
            {
                { "request", "The entire request object is missing or invalid." },
                { "Name", "Full Name is required and cannot be empty." },
                { "Email", "Email Address is Empty." },
                { "PasswordHash", "Password is required." },
                { "Phone", "Phone number is required for contact." },
                { "RoleID", "A valid User Role must be selected." }
            };
        }

        public static class Crisis
        {
            // General
            public const string RequestNull = "Crisis request cannot be null.";
            public const string CrisisNotFound = "Crisis not found.";
            public const string DateRequired = " Date is required";
            public const string InvalidType = "Invalid crisis type";
            public const string TypeRequired = "Crisis type is required";
            public const string SeverityRequired = "Crisis severity is required";
            public const string DeclarationFailed = "Failed to declare crisis.";
            public const string LocationRequired = "Crisis location is required.";
            public const string InvalidDate = "A valid crisis date is required.";
            public const string InvalidSeverity = "Invalid crisis severity.";
            public const string InvalidStatus = "Invalid crisis status.";
            public const string Duplicate="Crisis already exists for given details...";
            public static readonly Dictionary<string, string> Field = new()
            {
                { "Location", LocationRequired },
                { "Date", InvalidDate },
                { "Severity", InvalidSeverity },
                { "Status", InvalidStatus },
                { "Type", InvalidType }
            };
        }

        public static class ForgotPassword
        {
            public const string UserNotFound = "No user found with the provided email address.";
            public const string ProcessingFailed = "An error occurred while processing the forgot password request.";
            public const string SameAsOldPassword = "The new password must be different from the old password.";
            public const string PasswordMismatch = "Password and Confirm Password do not match.";

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

        public static class Login
        {
            public const string EmailRequired = "Email address is required.";
            public const string PasswordRequired = "Password is required.";
        }

        public static class UserDelete
        {
            public const string InvalidUserId = "User ID must be a positive number.";
            public const string UserNotFound = "No active user found with the provided ID.";
            public const string DeleteSuccess = "User has been successfully deleted.";
            public const string AdminCannotBeDeleted = "Admin users cannot be deleted directly. Please update the user's role, then retry the delete.";
        }
    }
}