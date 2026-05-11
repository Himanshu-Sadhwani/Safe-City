using System.ComponentModel.DataAnnotations;

namespace SafeCity.Utility
{
    /// <summary>
    /// Validation attribute that ensures a date value is exactly today.
    /// Rejects both past and future dates with separate configurable messages.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class FutureOrTodayDateAttribute : ValidationAttribute
    {
        /// <summary>
        /// Error message returned when the date is in the future.
        /// Defaults to "Date cannot be in the future." if not set.
        /// </summary>
        public string FutureDateErrorMessage { get; set; } = "Date cannot be in the future.";

        /// <summary>
        /// Validates that the date is today. Returns a distinct error message
        /// for past dates (<see cref="ValidationAttribute.ErrorMessage"/>) and
        /// future dates (<see cref="FutureDateErrorMessage"/>).
        /// </summary>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime date)
            {
                if (date.Date < DateTime.Today)
                    return new ValidationResult(ErrorMessage ?? "Date cannot be in the past.");

                if (date.Date > DateTime.Today)
                    return new ValidationResult(FutureDateErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
