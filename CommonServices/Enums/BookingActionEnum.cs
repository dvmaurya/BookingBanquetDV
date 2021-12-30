using System.ComponentModel.DataAnnotations;

namespace CommonServices.Enums
{
    public enum BookingActionEnum
    {
        [Display(Description = "None")]
        None,
        [Display(Description = "View booking")]
        ViewBooking = 101,
        [Display(Description = "Edit booking")]
        EditBooking = 102,
        [Display(Description = "Delete booking")]
        DeleteBooking = 103,
        [Display(Description = "Send an email")]
        SendEmail = 104,
        [Display(Description = "Take a printout")]
        TakeAPrintout = 105,
    }
}
