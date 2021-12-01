using CommonServices.ConstantData;
using CommonServices.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ViewModelsAndValidation.ViewModels;

namespace BookingBanquetDV.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DateRequiredAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && value is DateTimeOffset dateTime)
            {
                if (dateTime.Equals(DateTimeOffset.MinValue))
                    return new ValidationResult(this.ErrorMessageString);
                
                if (validationContext.ObjectInstance is INewBookingViewModel newBooking)
                {
                    if (newBooking.BookingParameter != null)
                    {
                        IList<string> allBookingDates =
                            newBooking.BookingParameter.MasterBookingList?.Select(date => date.BookingDate).ToList();

                        if (newBooking.BookingParameter.IsEditBooking)
                        {
                            if(newBooking.BookingParameter.BookingDetailList.FirstOrDefault(
                                booking =>
                                booking.Id != newBooking.BookingParameter.BookingDetails?.Id &&
                                booking.BookingDate.FormatStringEquals(newBooking.BookingParameter.BookingDetails.BookingDate)) != null)
                                return CheckExistingDate(allBookingDates, dateTime);
                        }
                        else
                        {
                            return CheckExistingDate(allBookingDates, dateTime);
                        }
                    }
                }
            }

            return ValidationResult.Success;
        }

        private ValidationResult CheckExistingDate(IList<string> allBookingDates, DateTimeOffset dateTime)
        {
            string existDate = allBookingDates?.FirstOrDefault(
                date => date.FormatStringEquals(
                    dateTime.ConvertToString(ApplicationConstants.DatabaseDateFormat)));

            if (!string.IsNullOrEmpty(existDate))
                return new ValidationResult(ApplicationConstants.DateBooked);

            return ValidationResult.Success;
        }
    }
}
