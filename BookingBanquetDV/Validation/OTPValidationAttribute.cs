using CommonServices.ConstantData;
using CommonServices.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using ViewModelsAndValidation.ViewModels;

namespace BookingBanquetDV.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class OTPValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (validationContext != null &&
                validationContext.ObjectInstance is IRecoverUserViewModel recoverUser)
            {
                if(recoverUser.IsRecoverEnable)
                {
                    if (string.IsNullOrEmpty(recoverUser.OTP))
                        return new ValidationResult(ApplicationConstants.OTPRequired);
                    else if (!recoverUser.OTP.FormatStringEquals(recoverUser.GeneratedOTP))
                        return new ValidationResult(this.ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}
