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
    public class PhotoDetailRepository : IPhotoDetailRepository
    {
        private string _dbpath;
        private static string _fileName = "Filename={0}";

        public void AddOrUpdatePhotoDetail(PhotoDetails photoDetail)
        {
            try
            {
                _dbpath = DataAccess.GetDbPath();

                string insertOrUpdate = photoDetail.Id == 0
                    ?
                    PhotoDetailQuery.InsertInPhotoDetail
                    :
                    PhotoDetailQuery.UpdatePhotoDetail;

                using (SqliteConnection db = new SqliteConnection(_fileName.FormatString(_dbpath)))
                {
                    db.Open();

                    SqliteCommand insertCommand = new SqliteCommand
                    {
                        Connection = db,
                        CommandText = insertOrUpdate
                    };

                    insertCommand.Parameters.AddWithValue("@" + nameof(PhotoDetails.Id), photoDetail.Id);
                    insertCommand.Parameters.AddWithValue("@" + nameof(PhotoDetails.PhotoName), photoDetail.PhotoName);
                    insertCommand.Parameters.AddWithValue("@" + nameof(PhotoDetails.Date), photoDetail.Date);
                    insertCommand.Parameters.AddWithValue("@" + nameof(PhotoDetails.FileType), photoDetail.FileType);
                    insertCommand.Parameters.AddWithValue("@" + nameof(PhotoDetails.Size), photoDetail.Size);
                    insertCommand.Parameters.AddWithValue("@" + nameof(PhotoDetails.EmailId), photoDetail.EmailId);
                    insertCommand.Parameters.AddWithValue("@" + nameof(PhotoDetails.IsPhotoActive), photoDetail.IsPhotoActive);
                    insertCommand.Parameters.AddWithValue("@" + nameof(PhotoDetails.IsPhotoRemoved), photoDetail.IsPhotoRemoved);

                    insertCommand.ExecuteReader();
                    db.Close();
                }
            }
            catch (Exception ex)
            {
                FileLogger.Logger.Info("Photo Exception: ".ConcatString(ex.Message));
            }
        }

        public IList<PhotoDetails> GetPreviousPhotoDetail(string emailId)
        {
            IList<PhotoDetails> photoDetails = new List<PhotoDetails>();
            PhotoDetails photoDetail = null;
            _dbpath = DataAccess.GetDbPath();

            using (SqliteConnection db = new SqliteConnection(_fileName.FormatString(_dbpath)))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand
                    (PhotoDetailQuery.GetPreviousPhoto.FormatString(emailId), db);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    photoDetail = new PhotoDetails
                    {
                        Id = query.GetString(0).ToInt32(),
                        PhotoName = query.GetString(1),
                        Date = query.GetString(2),
                        FileType = query.GetString(3),
                        Size = query.GetString(4).UlongParse(),
                        EmailId = query.GetString(5),
                        IsPhotoActive = query.GetBoolean(6),
                        IsPhotoRemoved = query.GetBoolean(7)
                    };

                    photoDetails.Add(photoDetail);
                }

                db.Close();
            }

            return photoDetails;
        }

        public PhotoDetails GetActivePhotoDetail(string emailId)
        {
            PhotoDetails photoDetail = null;
            _dbpath = DataAccess.GetDbPath();

            using (SqliteConnection db = new SqliteConnection(_fileName.FormatString(_dbpath)))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand
                    (PhotoDetailQuery.GetActivePhoto.FormatString(emailId), db);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    photoDetail = new PhotoDetails
                    {
                        Id = query.GetString(0).ToInt32(),
                        PhotoName = query.GetString(1),
                        Date = query.GetString(2),
                        FileType = query.GetString(3),
                        Size = query.GetString(4).UlongParse(),
                        EmailId = query.GetString(5),
                        IsPhotoActive = query.GetBoolean(6),
                        IsPhotoRemoved = query.GetBoolean(7)
                    };
                }

                db.Close();
            }

            return photoDetail;
        }

        public async Task DeleteDocumentFileFromFolderAsync(string photoName)
        {
            try
            {
                if (!string.IsNullOrEmpty(photoName))
                {
                    StorageFolder localfolder =
                        await ApplicationData.Current.LocalFolder.GetFolderAsync(
                            ApplicationConstants.Document);

                    if (await localfolder.TryGetItemAsync(photoName) != null)
                    {
                        StorageFile isFileExist = await localfolder.GetFileAsync(photoName);
                        await isFileExist.DeleteAsync();
                    }
                }
            }
            catch(Exception ex)
            {
                FileLogger.Logger.Info("Photo delete from folder exception: ".ConcatString(ex.Message));
            }
        }
    }
}
