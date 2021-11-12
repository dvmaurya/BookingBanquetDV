using System.ComponentModel.DataAnnotations;
using ViewModelsAndValidation.ViewModels;

namespace BookingBanquetDV.Validation
{
    public class NameValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (validationContext != null &&
                validationContext.ObjectInstance is ISignupViewModel signup &&
                !signup.IsOtpEnable &&
                string.IsNullOrEmpty(signup.FullName))
            {
                return new ValidationResult(this.ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
