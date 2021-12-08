using BookingServices.Repository.Interface;
using CommonServices.ConstantData;
using CommonServices.Extensions;
using CommonServices.Logger;
using CommonServices.Model.DbModel;
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
                    insertCommand.Parameters.AddWithValue("@" + nameof(bookingDetails.PhotoName), bookingDetails.PhotoName);
                    insertCommand.Parameters.AddWithValue("@" + nameof(bookingDetails.UserId), bookingDetails.UserId);
                    insertCommand.Parameters.AddWithValue("@" + nameof(bookingDetails.IsEmailSent), bookingDetails.IsEmailSent);

                    insertCommand.ExecuteReader();

                    db.Close();
                }
            }
            catch (Exception ex)
            {
                FileLogger.Logger.Info("Exception: ".ConcatString(ex.Message));
            }
        }

        public IList<BookingDetails> GetAllBookings()
        {
            IList<BookingDetails> bookingDetails = new List<BookingDetails>();
            BookingDetails bookingDetail;
            _dbpath = DataAccess.GetDbPath();

            using (SqliteConnection db = new SqliteConnection(_fileName.FormatString(_dbpath)))
            {
                db.Open();

                string sqlText = BookingQuery.SelectAllFromBooking;

                SqliteCommand selectCommand = new SqliteCommand
                    (sqlText, db);

                SqliteDataReader query = selectCommand.ExecuteReader();

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
                        PhotoName = query.GetString(13),
                        UserId = query.GetString(14),
                        IsEmailSent = query.GetBoolean(15)
                    };

                    bookingDetails.Add(bookingDetail);
                }

                db.Close();
            }

            return bookingDetails;
        }

        public BookingDetails GetBookingById(int id)
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
