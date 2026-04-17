using CommonServices.Model.DbModel;
using System.Collections.Generic;

namespace BookingServices.Repository.Interface
{
    public interface IBookingRepository
    {
        void AddOrUpdateBooking(BookingDetails bookingDetails);
        IList<BookingDetails> GetAllBookings();
        void DeleteBooking(BookingDetails bookingDetails);
    }
}
