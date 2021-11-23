using CommonServices.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingServices.Interfaces
{
    public interface IAddBooking
    {
        Task AddNewBooking(BookingDetails bookingDetails);
        IList<BookingDetails> GetAllBookings();
        BookingDetails GetBookingId(int id);
    }
}
