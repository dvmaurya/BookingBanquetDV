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
            await _localFolder.CreateFileAsync(
                ApplicationConstants.DatabaseName,
                CreationCollisionOption.OpenIfExists);

            GetDbPath();

            using (SqliteConnection db = new SqliteConnection(_fileName.FormatString(_dbpath)))
            {
                db.Open();
                string tableCommand = string.Empty;

                switch(tableName)
                {
                    case ApplicationConstants.Users:
                        tableCommand = SignupQuery.CreateSignupTable;
                        break;
                    case ApplicationConstants.LoginDetail:
                        tableCommand = LoginQuery.CreateLoginTable;
                        break;
                    case ApplicationConstants.BookingDetails:
                        tableCommand = BookingQuery.CreateBookingTable;
                        break;
                    default:
                        break;
                }

                SqliteCommand createTable = new SqliteCommand(tableCommand, db);

                createTable.ExecuteReader();
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
