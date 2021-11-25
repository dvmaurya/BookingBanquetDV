using System.Collections.ObjectModel;

namespace CommonServices.Model
{
    public class BookingParameter
    {
        public LoginDetail LoginDetail { get; set; }
        public BookingDetails BookingDetails { get; set; }
        public ObservableCollection<BookingDetails> BookingDetailList { get; set; }
        public bool IsViewBooking { get; set; }
        public bool IsEditBooking { get; set; }
    }
}
