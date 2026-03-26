using System.Net.Mail;
using System.Text.RegularExpressions;

namespace SafeCity.Utility
{
    public class EmailHelper
    {
        public static (bool IsValid, string Message) ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return (false, "Email address cannot be empty.");

            if (email.Length > 254)
                return (false, "Email is too long.");

            try
            {
                var addr = new MailAddress(email);

                if (addr.Address != email)
                    return (false, "Invalid email format.");

                string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(email, pattern))
                    return (false, "Email must have a valid domain.");

                return (true, "Email is valid.");
            }
            catch (FormatException)
            {
                return (false, "The email format is incorrect.");
            }
        }
    }
}