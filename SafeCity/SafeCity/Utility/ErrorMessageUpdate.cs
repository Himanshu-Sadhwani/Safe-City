using System;

namespace SafeCity.Utility
{
    public static class ErrorMessageUpdate
    {
        public static class User
        {
            public const string RequestNull = "Registration request cannot be null.";
            public const string RequiredFields = "Required fields are missing or invalid.";
            public const string EmailExists = "A user with this email already exists.";
            public const string InternalError = "An internal server error occurred.";
        }

        public static class UserUpdate
        {
            public const string RequestNull = "Update request cannot be null.";
            public const string InvalidUserId = "Invalid UserID.";
            public const string UserNotFound = "User not found.";
            public const string NameRequired = "Name is required.";
            public const string PhoneRequired = "Phone number is required.";
            public const string InvalidRoleId = "Invalid RoleID.";
            public const string UpdateFailed = "User update failed.";
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
        }
    }
}