using CommonServices.Enums;
using CommonServices.Model;

namespace BookingBanquetDV.Utility
{
    public class AddBookingEventArgs
    {
        private readonly BookingDetails _bookingDetails;
        private readonly PreviewDialogAction _previewDialogAction;

        public AddBookingEventArgs(
            BookingDetails bookingDetails,
            PreviewDialogAction previewDialogAction)
        {
            _bookingDetails = bookingDetails;
            _previewDialogAction = previewDialogAction;
        }

        public BookingDetails BookingDetails => _bookingDetails;

        public PreviewDialogAction PreviewDialogAction => _previewDialogAction;
    }
}
