using CommonServices.Model;
using System.Collections.Generic;

namespace BookingServices.Interfaces
{
    public interface IBookingDataProvider
    {
        void AddOrUpdateBooking(BookingDetails bookingDetails);
        IList<BookingDetails> GetAllBookings();
        BookingDetails GetBooking(int id);
        void DeleteBooking(BookingDetails bookingDetails);
    }
}
