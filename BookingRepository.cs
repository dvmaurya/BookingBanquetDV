using BookingServices.Repository.Interface;
using CommonServices.Analytcis;
using CommonServices.Extensions;
using CommonServices.Logger;
using CommonServices.Model.DbModel;
using DataAccessLibrary;
using DataAccessLibrary.DML;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace BookingServices.Repository
{
    public class BookingRepository : BaseRepository, IBookingRepository
    {
        private string _dbpath;
        private static string _fileName = "Filename={0}";

        public BookingRepository(
            IAnalyticsManager analyticsManager)
            : base(analyticsManager)
        {
        }

        public void AddOrUpdateBooking(BookingDetails bookingDetails)
        {
            string insertOrUpdate = string.Empty;
            try
            {
                _dbpath = DataAccess.GetDbPath();

                if (bookingDetails != null && bookingDetails.Id != 0)
                {
                    insertOrUpdate = BookingQuery.UpdateBookingDetail;
                }
                else
                {
                    insertOrUpdate = BookingQuery.InsertInBooking;
                }

                using (SqliteConnection db = new SqliteConnection(_fileName.FormatString(_dbpath)))
                {
                    db.Open();

                    using (SqliteCommand insertCommand = new SqliteCommand(insertOrUpdate, db))
                    {
                        insertCommand.Parameters.AddWithValue("@" + nameof(bookingDetails.Id), bookingDetails.Id);
                        insertCommand.Parameters.AddWithValue("@" + nameof(bookingDetails.CustomerName), bookingDetails.CustomerName);
                        insertCommand.Parameters.AddWithValue("@" + nameof(bookingDetails.MobileNo), bookingDetails.MobileNo);
                        insertCommand.Parameters.AddWithValue("@" + nameof(bookingDetails.EmailId), bookingDetails.EmailId);
                        insertCommand.Parameters.AddWithValue("@" + nameof(bookingDetails.BookingDate), bookingDetails.BookingDate);
                        insertCommand.Parameters.AddWithValue("@" + nameof(bookingDetails.BookedBy), bookingDetails.BookedBy);
                        insertCommand.Parameters.AddWithValue("@" + nameof(bookingDetails.TotalRooms), bookingDetails.TotalRooms);
                        insertCommand.Parameters.AddWithValue("@" + nameof(bookingDetails.TotalFlours), bookingDetails.TotalFlours);
                        insertCommand.Parameters.AddWithValue("@" + nameof(bookingDetails.TotalAmount), bookingDetails.TotalAmount);
                        insertCommand.Parameters.AddWithValue("@" + nameof(bookingDetails.Advanced), bookingDetails.Advanced);
                        insertCommand.Parameters.AddWithValue("@" + nameof(bookingDetails.Balance), bookingDetails.Balance);
                        insertCommand.Parameters.AddWithValue("@" + nameof(bookingDetails.Confirmation), bookingDetails.Confirmation);
                        insertCommand.Parameters.AddWithValue("@" + nameof(bookingDetails.CreationDate), bookingDetails.CreationDate);
                        insertCommand.Parameters.AddWithValue("@" + nameof(bookingDetails.UserId), bookingDetails.UserId);
                        insertCommand.Parameters.AddWithValue("@" + nameof(bookingDetails.IsEmailSent), bookingDetails.IsEmailSent);
                        insertCommand.Parameters.AddWithValue("@" + nameof(bookingDetails.Address), bookingDetails.Address);
                        insertCommand.Parameters.AddWithValue("@" + nameof(bookingDetails.Country), bookingDetails.Country);
                        insertCommand.Parameters.AddWithValue("@" + nameof(bookingDetails.State), bookingDetails.State);
                        insertCommand.Parameters.AddWithValue("@" + nameof(bookingDetails.City), bookingDetails.City);

                        insertCommand.ExecuteReader();
                    }

                    db.Close();

                    TraceEvents(
                        analyticsEvent: AnalyticConstants.Application,
                        action: $"{AnalyticConstants.Banquet} " +
                                $"{AnalyticConstants.Booking} ",
                        label: $"{AnalyticConstants.Banquet} " +
                                $"{AnalyticConstants.Booking} " +
                                $"{AnalyticConstants.Added} " +
                                $"{AnalyticConstants.Successfully}");
                }
            }
            catch (Exception ex)
            {
                FileLogger.Logger.Info("Add booking exception: ".ConcatString(ex.Message));
                TraceExceptions("Add booking exception: ".ConcatString(ex.Message));
            }
        }

        public IList<BookingDetails> GetAllBookings()
        {
            IList<BookingDetails> bookingDetails = new List<BookingDetails>();
            BookingDetails bookingDetail;
            _dbpath = DataAccess.GetDbPath();

            try
            {
                using (SqliteConnection db = new SqliteConnection(_fileName.FormatString(_dbpath)))
                {
                    db.Open();

                    string sqlText = BookingQuery.SelectAllFromBooking;

                    using (SqliteCommand selectCommand = new SqliteCommand(sqlText, db))
                    {
                        using (SqliteDataReader query = selectCommand.ExecuteReader())
                        {
                            while (query.Read())
                            {
                                bookingDetail = new BookingDetails
                                {
                                    Id = query.GetString(0).ToInt32(),
                                    CustomerName = query.GetString(1),
                                    MobileNo = query.GetString(2),
                                    EmailId = query.GetString(3),
                                    BookingDate = query.GetString(4),
                                    BookedBy = query.GetString(5),
                                    TotalRooms = query.GetString(6),
                                    TotalFlours = query.GetString(7),
                                    TotalAmount = query.GetString(8),
                                    Advanced = query.GetString(9),
                                    Balance = query.GetString(10),
                                    Confirmation = query.GetString(11),
                                    CreationDate = query.GetString(12),
                                    UserId = query.GetString(13),
                                    IsEmailSent = query.GetBoolean(14),
                                    Address = query.GetString(15),
                                    Country = query.GetString(16),
                                    State = query.GetString(17),
                                    City = query.GetString(18)
                                };

                                bookingDetails.Add(bookingDetail);
                            }
                        }
                    }

                    db.Close();
                }
            }
            catch (Exception ex)
            {
                FileLogger.Logger.Info("Get booking detail from database Exception: ".ConcatString(ex.Message));
                TraceExceptions("Get booking detail from database Exception: ".ConcatString(ex.Message));
            }

            return bookingDetails;
        }

        public void DeleteBooking(BookingDetails bookingDetails)
        {
            _dbpath = DataAccess.GetDbPath();

            try
            {
                using (SqliteConnection db = new SqliteConnection(_fileName.FormatString(_dbpath)))
                {
                    db.Open();

                    string sqlText = BookingQuery.DeleteBooking.FormatString(bookingDetails.Id);

                    using (SqliteCommand selectCommand = new SqliteCommand(sqlText, db))
                    {
                        selectCommand.ExecuteReader();
                    }

                    db.Close();
                }
            }
            catch (Exception ex)
            {
                FileLogger.Logger.Info("Booking delete exception: ".ConcatString(ex.Message));
                TraceExceptions("Banquet delete exception: ".ConcatString(ex.Message));
            }
        }
    }
}
