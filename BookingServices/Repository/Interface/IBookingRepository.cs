using CommonServices.Model.DbModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingServices.Repository.Interface
{
    public interface IBookingRepository
    {
        void AddOrUpdateBooking(BookingDetails bookingDetails);
        IList<BookingDetails> GetAllBookings();
        Task DeleteBookingAsync(BookingDetails bookingDetails);
    }
}
