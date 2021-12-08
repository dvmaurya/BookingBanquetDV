using CommonServices.Model.DbModel;
using System.Collections.Generic;
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
        public IList<BookingDetails> MasterBookingList { get; set; }

    }
}
