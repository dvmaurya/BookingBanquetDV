using CommonServices.Model;
using System.Collections.Generic;

namespace BookingServices.Interfaces
{
    public interface IBookingDataProvider
    {
        void AddOrUpdateBooking(BookingDetails bookingDetails);
        IList<BookingDetails> GetAllBookings();
        void DeleteBooking(BookingDetails bookingDetails);
    }
}
