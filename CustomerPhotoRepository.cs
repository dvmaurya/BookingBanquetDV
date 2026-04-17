using BookingServices.Repository.Interface;
using CommonServices.Analytcis;
using CommonServices.ConstantData;
using CommonServices.Extensions;
using CommonServices.Logger;
using CommonServices.Model.DbModel;
using DataAccessLibrary;
using DataAccessLibrary.DML;
using Microsoft.Data.Sqlite;
using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace BookingServices.Repository
{
    public class CustomerPhotoRepository : BaseRepository, ICustomerPhotoRepository
    {
        private string _dbpath;
        private static string _fileName = "Filename={0}";

        public CustomerPhotoRepository(
            IAnalyticsManager analyticsManager) :
            base(analyticsManager)
        {

        }

        public void AddOrUpdatePhotoDetail(CustomerPhotoDetail photoDetail)
        {
            try
            {
                _dbpath = DataAccess.GetDbPath();

                string insertOrUpdate = photoDetail.Id == 0
                    ?
                    CustomerPhotoDetailQuery.InsertInPhotoDetail
                    :
                    CustomerPhotoDetailQuery.UpdatePhotoDetail;

                using (SqliteConnection db = new SqliteConnection(_fileName.FormatString(_dbpath)))
                {
                    db.Open();

                    using (SqliteCommand insertCommand = new SqliteCommand(insertOrUpdate, db))
                    {
                        insertCommand.Parameters.AddWithValue("@" + nameof(CustomerPhotoDetail.Id), photoDetail.Id);
                        insertCommand.Parameters.AddWithValue("@" + nameof(CustomerPhotoDetail.PhotoName), photoDetail.PhotoName);
                        insertCommand.Parameters.AddWithValue("@" + nameof(CustomerPhotoDetail.Date), photoDetail.Date);
                        insertCommand.Parameters.AddWithValue("@" + nameof(CustomerPhotoDetail.FileType), photoDetail.FileType);
                        insertCommand.Parameters.AddWithValue("@" + nameof(CustomerPhotoDetail.Size), photoDetail.Size);
                        insertCommand.Parameters.AddWithValue("@" + nameof(CustomerPhotoDetail.EmailId), photoDetail.EmailId);
                        insertCommand.Parameters.AddWithValue("@" + nameof(CustomerPhotoDetail.BookingId), photoDetail.BookingId);

                        insertCommand.ExecuteReader();
                    }

                    db.Close();

                    TraceEvents(
                        analyticsEvent: AnalyticConstants.Application,
                        action: $"{AnalyticConstants.Customer} " +
                                $"{AnalyticConstants.Photo} ",
                        label: $"{AnalyticConstants.Customer} " +
                                $"{AnalyticConstants.Photo} " +
                                $"{AnalyticConstants.Added} " +
                                $"in {AnalyticConstants.Database} " +
                                $"{AnalyticConstants.Successfully} ");
                }
            }
            catch (Exception ex)
            {
                FileLogger.Logger.Info("Customer Photo insert/update Exception: ".ConcatString(ex.Message));
                TraceExceptions("Customer Photo insert/update Exception: ".ConcatString(ex.Message));

            }
        }

        public CustomerPhotoDetail GetCustomerPhotoDetail(int bookingId)
        {
            CustomerPhotoDetail photoDetail = null;

            try
            {
                _dbpath = DataAccess.GetDbPath();

                using (SqliteConnection db = new SqliteConnection(_fileName.FormatString(_dbpath)))
                {
                    db.Open();

                    using (SqliteCommand selectCommand = new SqliteCommand
                        (CustomerPhotoDetailQuery.GetActivePhoto.FormatString(bookingId), db))
                    {
                        using (SqliteDataReader query = selectCommand.ExecuteReader())
                        {
                            while (query.Read())
                            {
                                photoDetail = new CustomerPhotoDetail
                                {
                                    Id = query.GetInt32(0),
                                    PhotoName = query.GetString(1),
                                    Date = query.GetString(2),
                                    FileType = query.GetString(3),
                                    Size = query.GetString(4).UlongParse(),
                                    EmailId = query.GetString(5),
                                    BookingId = query.GetInt32(6)
                                };
                            }
                        }
                    }

                    db.Close();
                }
            }
            catch(Exception ex)
            {
                FileLogger.Logger.Info("Customer photo retrieve exception: ".ConcatString(ex.Message));
                TraceExceptions("Coustomer photo retrieve exception: ".ConcatString(ex.Message));
            }

            return photoDetail;
        }

        public async Task DeleteCustomerPhotoAsync(CustomerPhotoDetail photoDetail)
        {
            try
            {
                _dbpath = DataAccess.GetDbPath();

                if (photoDetail != null)
                {
                    using (SqliteConnection db = new SqliteConnection(_fileName.FormatString(_dbpath)))
                    {
                        db.Open();
                        string sqlText = CustomerPhotoDetailQuery.DeleteCustomerPhoto.FormatString(photoDetail.BookingId);

                        using (SqliteCommand selectCommand = new SqliteCommand(sqlText, db))
                        {
                            SqliteDataReader query = selectCommand.ExecuteReader();
                        }

                        db.Close();
                    }
                }

                await DeleteCustomerPhotoFromFolderAsync(photoDetail.PhotoName).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                FileLogger.Logger.Info("Customer photo delete exception: ".ConcatString(ex.Message));
                TraceExceptions("Customer photot delete exception: ".ConcatString(ex.Message));
            }
        }

        private async Task DeleteCustomerPhotoFromFolderAsync(string photoName)
        {
            try
            {
                if (!string.IsNullOrEmpty(photoName))
                {
                    StorageFolder localfolder =
                        await ApplicationData.Current.LocalFolder.GetFolderAsync(
                            ApplicationConstants.CustomerPhoto);

                    if (await localfolder.TryGetItemAsync(photoName) != null)
                    {
                        StorageFile isFileExist = await localfolder.GetFileAsync(photoName);
                        await isFileExist.DeleteAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogger.Logger.Info("Customer photo delete from folder exception: ".ConcatString(ex.Message));
                TraceExceptions("Customer photo delete from folder exception: ".ConcatString(ex.Message));
            }
        }
    }
}
