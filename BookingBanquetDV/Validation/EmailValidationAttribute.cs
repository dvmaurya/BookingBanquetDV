using CommonServices.ConstantData;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using ViewModelsAndValidation.ViewModels;

namespace BookingBanquetDV.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class EmailValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (validationContext != null)
            {
                Regex regex = new Regex(ApplicationConstants.EmailRegex);

                if (validationContext.ObjectInstance is ISignupViewModel signup)
                {
                    if (string.IsNullOrEmpty(signup.EmailId))
                        return new ValidationResult(ApplicationConstants.EmailRequired);
                    else if (!regex.IsMatch(signup.EmailId))
                        return new ValidationResult(this.ErrorMessage);
                    else if(signup.IsEmailExist)
                        return new ValidationResult(ApplicationConstants.EmailIdExist);
                }
                else if (validationContext.ObjectInstance is IMainViewModel mainView)
                {
                    if (string.IsNullOrEmpty(mainView.UserId))
                        return new ValidationResult(this.ErrorMessage);
                }
                else if (validationContext.ObjectInstance is IRecoverUserViewModel recoverUser)
                {
                    if (string.IsNullOrEmpty(recoverUser.EmailId))
                        return new ValidationResult(ApplicationConstants.EmailRequired);
                    else if (!regex.IsMatch(recoverUser.EmailId))
                        return new ValidationResult(this.ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}
