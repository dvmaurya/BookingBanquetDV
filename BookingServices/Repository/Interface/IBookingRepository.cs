using CommonServices.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingServices.Repository.Interface
{
    public interface IBookingRepository
    {
        void AddOrUpdateBooking(BookingDetails bookingDetails);
        IList<BookingDetails> GetAllBookings();
        BookingDetails GetBooking(int id);
        Task DeleteBookingAsync(BookingDetails bookingDetails);
    }
}
