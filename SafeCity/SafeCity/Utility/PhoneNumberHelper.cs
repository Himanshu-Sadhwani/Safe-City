using System.Text.RegularExpressions;

namespace SafeCity.Utility
{
    public static class PhoneNumberHelper
    {
        // Validates if the phone number is exactly 10 digits
        public static bool IsValidPhoneNumber(string? phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;
            // Adjust regex as per your requirements
            return Regex.IsMatch(phoneNumber, @"^\d{10}$");
        }
    }
}
