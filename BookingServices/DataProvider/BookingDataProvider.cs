using BookingServices.Interfaces;
using BookingServices.Repository.Interface;
using CommonServices.Model;
using System;
using System.Collections.Generic;

namespace BookingServices.DataProvider
{
    public class BookingDataProvider : IBookingDataProvider
    {
        private readonly IBookingRepository _addBookingRepository;

        public BookingDataProvider(
            IBookingRepository addBookingRepository)
        {
            _addBookingRepository = addBookingRepository ?? throw new ArgumentNullException(nameof(addBookingRepository));
        }

        public void AddOrUpdateBooking(BookingDetails bookingDetails)
        {
            _addBookingRepository.AddOrUpdateBooking(bookingDetails);
        }

        public void DeleteBooking(BookingDetails bookingDetails)
        {
            _addBookingRepository.DeleteBookingAsync(bookingDetails);
        }

        public IList<BookingDetails> GetAllBookings()
        {
            return _addBookingRepository.GetAllBookings();
        }

        public BookingDetails GetBooking(int id)
        {
            return _addBookingRepository.GetBooking(id);
        }
    }
}
