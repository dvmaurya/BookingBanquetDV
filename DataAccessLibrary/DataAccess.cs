using CommonServices.ConstantData;
using CommonServices.Extensions;
using DataAccessLibrary.DML;
using Microsoft.Data.Sqlite;
using System;
using System.IO;
using Windows.Storage;

namespace DataAccessLibrary
{
    public static class DataAccess
    {
        private static StorageFolder _localFolder = ApplicationData.Current.LocalFolder;
        private static string _dbpath;
        private static string _fileName = "Filename={0}";

        public async static void CreateTables(string tableName)
        {
            GetDbPath();

            using (SqliteConnection db = new SqliteConnection(_fileName.FormatString(_dbpath)))
            {
                db.Open();
                string tableCommand = string.Empty;

                switch(tableName)
                {
                    case TableName.UsersCred:
                        await _localFolder.CreateFileAsync(
                        ApplicationConstants.DatabaseName,
                        CreationCollisionOption.OpenIfExists);

                        tableCommand = SignupQuery.CreateSignupTable;
                        break;
                    case TableName.UsersPersonalDetail:
                        tableCommand = UsersPersonalDetailQuery.CreateUsersPersonalDetailTable;
                        break;
                    case TableName.LoginDetail:
                        tableCommand = LoginQuery.CreateLoginTable;
                        break;
                    case TableName.BookingDetails:
                        tableCommand = BookingQuery.CreateBookingTable;
                        break;
                    case TableName.Country:
                        tableCommand = CountryQuery.CreateCountryTable;
                        break;
                    case TableName.PhotoDetails:
                        tableCommand = PhotoDetailQuery.CreatePhotoDetailTable;
                        break;
                    default:
                        break;
                }

                if (!string.IsNullOrEmpty(tableCommand))
                {
                    SqliteCommand createTable = new SqliteCommand(tableCommand, db);

                    createTable.ExecuteReader();
                }
                db.Close();
            }
        }

        public static string GetDbPath()
        {
            _dbpath = Path.Combine(_localFolder.Path, ApplicationConstants.DatabaseName);

            return _dbpath;
        }
    }
}
