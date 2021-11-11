using CommonServices.ConstantData;
using System;
using System.ComponentModel.DataAnnotations;
using ViewModelsAndValidation.ViewModels;

namespace BookingBanquetDV.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PasswordValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (validationContext != null)
            {
                if (validationContext.ObjectInstance is ISignupViewModel signup)
                {
                    if (string.IsNullOrEmpty(signup.Password))
                        return new ValidationResult(ApplicationConstants.PasswordRequired);
                    else if (signup.Password.Length < 3)
                        return new ValidationResult(this.ErrorMessage);
                }
                else if (validationContext.ObjectInstance is IMainViewModel mainView)
                {
                    if (string.IsNullOrEmpty(mainView.Password))
                        return new ValidationResult(this.ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}
