using BookingServices.Repository.Interface;
using CommonServices.ConstantData;
using CommonServices.Extensions;
using CommonServices.Logger;
using CommonServices.Model;
using DataAccessLibrary;
using DataAccessLibrary.DML;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace BookingServices.Repository
{
    public class BookingRepository : IBookingRepository
    {
        private string _dbpath;
        private static string _fileName = "Filename={0}";

        public void AddOrUpdateBooking(BookingDetails bookingDetails)
        {
            string InsertOrUpdate = string.Empty;
            try
            {
                _dbpath = DataAccess.GetDbPath();

                if (bookingDetails != null && bookingDetails.Id != 0)
                {
                    InsertOrUpdate = BookingQuery.UpdateBookingDetail;
                }
                else
                {
                    InsertOrUpdate = BookingQuery.InsertInBooking;
                }

                using (SqliteConnection db = new SqliteConnection(_fileName.FormatString(_dbpath)))
                {
                    db.Open();

                    SqliteCommand insertCommand = new SqliteCommand
                    {
                        Connection = db,
                        CommandText = InsertOrUpdate
                    };

                    insertCommand.Parameters.AddWithValue("@" + nameof(bookingDetails.Id), bookingDetails.Id);
                    insertCommand.Parameters.AddWithValue("@" + nameof(bookingDetails.CustomerName), bookingDetails.CustomerName);
                    insertCommand.Parameters.AddWithValue("@" + nameof(bookingDetails.BookingDate), bookingDetails.BookingDate);
                    insertCommand.Parameters.AddWithValue("@" + nameof(bookingDetails.BookedBy), bookingDetails.BookedBy);
                    insertCommand.Parameters.AddWithValue("@" + nameof(bookingDetails.NumberOfRooms), bookingDetails.NumberOfRooms);
                    insertCommand.Parameters.AddWithValue("@" + nameof(bookingDetails.NumberOfFlours), bookingDetails.NumberOfFlours);
                    insertCommand.Parameters.AddWithValue("@" + nameof(bookingDetails.TotalAmount), bookingDetails.TotalAmount);
                    insertCommand.Parameters.AddWithValue("@" + nameof(bookingDetails.Advanced), bookingDetails.Advanced);
                    insertCommand.Parameters.AddWithValue("@" + nameof(bookingDetails.Balance), bookingDetails.Balance);
                    insertCommand.Parameters.AddWithValue("@" + nameof(bookingDetails.Confirmation), bookingDetails.Confirmation);
                    insertCommand.Parameters.AddWithValue("@" + nameof(bookingDetails.CreationDate), bookingDetails.CreationDate);
                    insertCommand.Parameters.AddWithValue("@" + nameof(bookingDetails.PhotoName), bookingDetails.PhotoName);
                    insertCommand.Parameters.AddWithValue("@" + nameof(bookingDetails.UserId), bookingDetails.UserId);

                    insertCommand.ExecuteReader();

                    db.Close();
                }
            }
            catch (Exception ex)
            {
                FileLogger.Logger.Info("Exception: ".ConcatString(ex.Message));
            }
        }

        public IList<BookingDetails> GetAllBookings(string userId)
        {
            IList<BookingDetails> bookingDetails = new List<BookingDetails>();
            BookingDetails bookingDetail;
            _dbpath = DataAccess.GetDbPath();

            using (SqliteConnection db = new SqliteConnection(_fileName.FormatString(_dbpath)))
            {
                db.Open();

                string sqlText = BookingQuery.SelectAllFromBooking.FormatString(userId);

                SqliteCommand selectCommand = new SqliteCommand
                    (sqlText, db);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    bookingDetail = new BookingDetails
                    {
                        Id = query.GetString(0).ToInt32(),
                        CustomerName = query.GetString(1),
                        BookingDate = query.GetString(2),
                        BookedBy = query.GetString(3),
                        NumberOfRooms = query.GetString(4),
                        NumberOfFlours = query.GetString(5),
                        TotalAmount = query.GetString(6),
                        Advanced = query.GetString(7),
                        Balance = query.GetString(8),
                        Confirmation = query.GetString(9),
                        CreationDate = query.GetString(10),
                        PhotoName = query.GetString(11),
                        UserId = query.GetString(12)
                    };

                    bookingDetails.Add(bookingDetail);
                }

                db.Close();
            }

            return bookingDetails;
        }

        public BookingDetails GetBooking(int id)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteBookingAsync(BookingDetails bookingDetails)
        {
            _dbpath = DataAccess.GetDbPath();

            using (SqliteConnection db = new SqliteConnection(_fileName.FormatString(_dbpath)))
            {
                db.Open();

                string sqlText = BookingQuery.DeleteBooking.FormatString(bookingDetails.Id);

                SqliteCommand selectCommand = new SqliteCommand
                    (sqlText, db);

                SqliteDataReader query = selectCommand.ExecuteReader();

                db.Close();
                await DeleteDocumentFileFromFolderAsync(bookingDetails).ConfigureAwait(true);
            }
        }

        private async Task DeleteDocumentFileFromFolderAsync(BookingDetails bookingDetails)
        {
            if (bookingDetails != null)
            {
                StorageFolder localfolder =
                    await ApplicationData.Current.LocalFolder.GetFolderAsync(
                        ApplicationConstants.Document);

                if (await localfolder.TryGetItemAsync(bookingDetails.PhotoName) != null)
                {
                    StorageFile isFileExist = await localfolder.GetFileAsync(bookingDetails.PhotoName);
                    await isFileExist.DeleteAsync();
                }
            }
        }
    }
}
