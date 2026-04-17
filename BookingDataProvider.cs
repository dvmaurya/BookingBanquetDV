using BookingServices.Interfaces;
using BookingServices.Repository.Interface;
using CommonServices.Extensions;
using CommonServices.Model;
using System;
using System.Collections.Generic;
using DbModel = CommonServices.Model.DbModel;

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
            _addBookingRepository.AddOrUpdateBooking(bookingDetails.ToDbBookingDetails());
        }

        public void DeleteBooking(BookingDetails bookingDetails)
        {
            _addBookingRepository.DeleteBooking(bookingDetails.ToDbBookingDetails());
        }

        public IList<BookingDetails> GetAllBookings()
        {
            IList<DbModel.BookingDetails> getAllbooking = _addBookingRepository.GetAllBookings();
            IList<BookingDetails> bookingDetailList = new List<BookingDetails>();

            foreach (DbModel.BookingDetails getBooking in getAllbooking)
                bookingDetailList.Add(getBooking.ToBookingDetails());

            return bookingDetailList;
        }
    }
}
