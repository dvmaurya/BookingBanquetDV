using CommonServices.ConstantData;
using CommonServices.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using ViewModelsAndValidation.ViewModels;

namespace BookingBanquetDV.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PasswordMatchValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (validationContext != null &&
                 validationContext.ObjectInstance is ISignupViewModel signup)
            {
                if (string.IsNullOrEmpty(signup.ConfirmPassword))
                    return new ValidationResult(ApplicationConstants.ConfirmPassword);
                else if (!signup.Password.FormatStringEquals(signup.ConfirmPassword))
                    return new ValidationResult(this.ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
