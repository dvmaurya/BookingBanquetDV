using BookingBanquetDV.Source.Register;
using System;
using System.ComponentModel.DataAnnotations;

namespace BookingBanquetDV.Converters
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class AddressValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (validationContext != null &&
                validationContext.ObjectInstance is INotesValidator notesValidator)
            {
                if (value != null &&
                    !string.IsNullOrEmpty(value.ToString()) &&
                    value.ToString().Length > notesValidator.MaxLength)
                    return new ValidationResult(string.Empty);
            }


            return ValidationResult.Success;
        }
    }
}
