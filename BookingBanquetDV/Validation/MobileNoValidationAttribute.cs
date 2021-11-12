using CommonServices.ConstantData;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using ViewModelsAndValidation.ViewModels;

namespace BookingBanquetDV.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class MobileNoValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (validationContext != null)
            {
                Regex regex = new Regex(ApplicationConstants.MobileRegex);

                if (validationContext.ObjectInstance is ISignupViewModel signup)
                {
                    if (!signup.IsOtpEnable)
                    {
                        if (string.IsNullOrEmpty(signup.MobileNo))
                            return new ValidationResult(ApplicationConstants.MobileRequired);
                        else if (!regex.IsMatch(signup.MobileNo))
                            return new ValidationResult(this.ErrorMessage);
                        else if (signup.IsMobileExist)
                            return new ValidationResult(ApplicationConstants.MobileExist);
                    }
                }
                else if (validationContext.ObjectInstance is IRecoverUserViewModel recoverUser)
                {
                    if (string.IsNullOrEmpty(recoverUser.MobileNo))
                        return new ValidationResult(ApplicationConstants.MobileRequired);
                    else if (!regex.IsMatch(recoverUser.MobileNo))
                        return new ValidationResult(this.ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}
