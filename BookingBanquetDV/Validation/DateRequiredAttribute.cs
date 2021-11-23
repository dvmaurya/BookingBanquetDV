using System;
using System.ComponentModel.DataAnnotations;

namespace BookingBanquetDV.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DateRequiredAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if (value.Equals(DateTimeOffset.MinValue))
                    return new ValidationResult(this.ErrorMessageString);
            }

            return ValidationResult.Success;
        }
    }
}
