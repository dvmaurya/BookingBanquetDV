using CommonServices.ConstantData;
using CommonServices.Enums;
using CommonServices.Extensions;
using CommonServices.Logger;
using CommonServices.Model;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Storage;

namespace DataAccessLibrary
{
    public static class DataAccess
    {
        private static StorageFolder _localFolder = ApplicationData.Current.LocalFolder;
        private static string _dbpath;
        private static string _fileName = "Filename={0}";

        public async static void InitializeDatabase(string tableName)
        {
            await _localFolder.CreateFileAsync(
                ApplicationConstants.DatabaseName,
                CreationCollisionOption.OpenIfExists);

            GetDbPath();

            using (SqliteConnection db = new SqliteConnection(_fileName.FormatString(_dbpath)))
            {
                db.Open();
                string tableCommand = string.Empty;

                if (tableName.FormatStringEquals(ApplicationConstants.Signup))
                    tableCommand = CreateSignupTableQuery();
                else if (tableName.FormatStringEquals(ApplicationConstants.Login))
                    tableCommand = CreateLoginTableQuery();

                SqliteCommand createTable = new SqliteCommand(tableCommand, db);

                createTable.ExecuteReader();
                db.Close();
            }
        }

        private static string CreateLoginTableQuery()
        {
            return "CREATE TABLE IF NOT " +
                "EXISTS " + nameof(LoginDetail) +
                " ( " + nameof(LoginDetail.Id) + " INTEGER PRIMARY KEY, " +
                " " + nameof(LoginDetail.UserName) + " NVARCHAR(2048) NUll," +
                " " + nameof(LoginDetail.UserId) + " NVARCHAR(2048) NUll," +
                " " + nameof(LoginDetail.IsRememberChecked) + " NVARCHAR(2048) NUll," +
                " " + nameof(LoginDetail.NumberOfLogin) + " NVARCHAR(2048) NUll," +
                " " + nameof(LoginDetail.LastLogin) + " NVARCHAR(2048) NUll)";
        }

        private static string CreateSignupTableQuery()
        {
            return "CREATE TABLE IF NOT " +
                "EXISTS " + nameof(Users) +
                " ( " + nameof(Users.Id) + " INTEGER PRIMARY KEY, " +
                " " + nameof(Users.UserId) + " NVARCHAR(2048) NUll," +
                " " + nameof(Users.FullName) + " NVARCHAR(2048) NUll," +
                " " + nameof(Users.EmailId) + " NVARCHAR(2048) NUll," +
                " " + nameof(Users.Mobile) + " NVARCHAR(2048) NUll," +
                " " + nameof(Users.Password) + " NVARCHAR(2048) NUll," +
                " " + nameof(Users.CreationDate) + " NVARCHAR(2048) NULL, " +
                " " + nameof(Users.IsActivate) + " NVARCHAR(2048) NULL)";
        }

        private static void GetDbPath()
        {
            _dbpath = Path.Combine(_localFolder.Path, ApplicationConstants.DatabaseName);
        }

        public static bool AddOrUpdateUsers(Users users)
        {
            try
            {
                string insertOrUpdate = string.Empty;
                GetDbPath();

                if(users.Id == 0)
                {
                    insertOrUpdate = "INSERT INTO " + nameof(users) + " VALUES (" +
                        "NULL, " +
                        "@" + nameof(users.UserId) + "," +
                        "@" + nameof(users.FullName) + "," +
                        "@" + nameof(users.EmailId) + "," +
                        "@" + nameof(users.Mobile) + "," +
                        "@" + nameof(users.Password) + "," +
                        "@" + nameof(users.CreationDate) + "," +
                        "0);";
                }
                else
                {
                    insertOrUpdate = "UPDATE " +
                        nameof(users) + " SET " +
                        nameof(users.UserId) + " = @" + nameof(users.UserId) + ", " +
                        nameof(users.FullName) + " = @" + nameof(users.FullName) + ", " +
                        nameof(users.EmailId) + " = @" + nameof(users.EmailId) + ", " +
                        nameof(users.Mobile) + " = @" + nameof(users.Mobile) + ", " +
                        nameof(users.Password) + " = @" + nameof(users.Password) + ", " +
                        nameof(users.CreationDate) + " = @" + nameof(users.CreationDate) + " Where " +
                        nameof(users.UserId) + " = @" + nameof(users.UserId);
                }
                using (SqliteConnection db = new SqliteConnection(_fileName.FormatString(_dbpath)))
                {
                    db.Open();

                    SqliteCommand insertCommand = new SqliteCommand
                    {
                        Connection = db,
                        CommandText = insertOrUpdate
                    };

                    insertCommand.Parameters.AddWithValue("@" + nameof(users.UserId), users.UserId);
                    insertCommand.Parameters.AddWithValue("@" + nameof(users.FullName), users.FullName);
                    insertCommand.Parameters.AddWithValue("@" + nameof(users.EmailId), users.EmailId);
                    insertCommand.Parameters.AddWithValue("@" + nameof(users.Mobile), users.Mobile);
                    insertCommand.Parameters.AddWithValue("@" + nameof(users.Password), users.Password);
                    insertCommand.Parameters.AddWithValue("@" + nameof(users.CreationDate), users.CreationDate);

                    insertCommand.ExecuteReader();
                    db.Close();

                    return true;
                }
            }
            catch(Exception ex)
            {
                FileLogger.Logger.Info("Exception: ".ConcatString(ex.Message));
                return false;
            }
        }

        public static bool AddOrUpdateLoginDetail(LoginDetail loginDetail)
        {
            string InsertOrUpdate = string.Empty;
            try
            {
                GetDbPath();

                IDictionary<string, string> userExist = GetLoginDetails(loginDetail);

                if(userExist != null && userExist.Any())
                {
                    loginDetail.NumberOfLogin = (userExist[nameof(LoginDetail.NumberOfLogin)].ToInt32() + 1).ConvertToString();
                    InsertOrUpdate = "UPDATE " +
                        nameof(LoginDetail) + " SET " +
                        nameof(LoginDetail.UserName) + " = @" + nameof(LoginDetail.UserName) + ", " +
                        nameof(LoginDetail.UserId) + " = @" + nameof(LoginDetail.UserId) + ", " +
                        nameof(LoginDetail.IsRememberChecked) + " = @" + nameof(LoginDetail.IsRememberChecked) + ", " +
                        nameof(LoginDetail.NumberOfLogin) + " = @" + nameof(LoginDetail.NumberOfLogin) + ", " +
                        nameof(LoginDetail.LastLogin) + " = @" + nameof(LoginDetail.LastLogin) + " Where " +
                        nameof(LoginDetail.UserId) + " = @" + nameof(LoginDetail.UserId);
                }
                else
                {
                    InsertOrUpdate = "INSERT INTO " + nameof(loginDetail) + " VALUES (" +
                        "NULL, " +
                        "@" + nameof(loginDetail.UserName) + "," +
                        "@" + nameof(loginDetail.UserId) + "," +
                        "@" + nameof(loginDetail.IsRememberChecked) + "," +
                        "@" + nameof(loginDetail.NumberOfLogin) + "," +
                        "@" + nameof(loginDetail.LastLogin) + ");";
                }

                using (SqliteConnection db = new SqliteConnection(_fileName.FormatString(_dbpath)))
                {
                    db.Open();

                    SqliteCommand insertCommand = new SqliteCommand
                    {
                        Connection = db,
                        CommandText = InsertOrUpdate
                    };

                    insertCommand.Parameters.AddWithValue("@" + nameof(loginDetail.UserName), loginDetail.UserName);
                    insertCommand.Parameters.AddWithValue("@" + nameof(loginDetail.UserId), loginDetail.UserId);
                    insertCommand.Parameters.AddWithValue("@" + nameof(loginDetail.IsRememberChecked), loginDetail.IsRememberChecked);
                    insertCommand.Parameters.AddWithValue("@" + nameof(loginDetail.NumberOfLogin), loginDetail.NumberOfLogin);
                    insertCommand.Parameters.AddWithValue("@" + nameof(loginDetail.LastLogin), loginDetail.LastLogin);

                    insertCommand.ExecuteReader();

                    db.Close();

                    return true;
                }
            }
            catch (Exception ex)
            {
                FileLogger.Logger.Info("Exception: ".ConcatString(ex.Message));

                return false;
            }
        }

        private static IDictionary<string, string> GetLoginDetails(LoginDetail loginDetail)
        {
            IDictionary<string, string> entries = new Dictionary<string, string>();
            GetDbPath();

            using (SqliteConnection db = new SqliteConnection(_fileName.FormatString(_dbpath)))
            {
                db.Open();

                string sqlText = "SELECT * from " +
                        nameof(LoginDetail) + " Where " +
                        nameof(LoginDetail.UserId) + " = '" + loginDetail.UserId + "'";

                SqliteCommand selectCommand = new SqliteCommand
                    (sqlText, db);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    entries.Add(nameof(LoginDetail.Id), query.GetString(0));
                    entries.Add(nameof(LoginDetail.UserName), query.GetString(1));
                    entries.Add(nameof(LoginDetail.UserId), query.GetString(2));
                    entries.Add(nameof(LoginDetail.IsRememberChecked), query.GetString(3));
                    entries.Add(nameof(LoginDetail.NumberOfLogin), query.GetString(4));
                    entries.Add(nameof(LoginDetail.LastLogin), query.GetString(5));
                }

                db.Close();
            }

            return entries;
        }

        public static List<string> GetAllUsers(string tableName)
        {
            List<string> entries = new List<string>();
            GetDbPath();

            using (SqliteConnection db = new SqliteConnection(_fileName.FormatString(_dbpath)))
            {
                db.Open();

                string sqlText = string.Empty;

                if (tableName.FormatStringEquals(ApplicationConstants.Signup))
                    sqlText = SelectTextForSignup();
                else if (tableName.FormatStringEquals(ApplicationConstants.Login))
                    sqlText = SelectTextForLogin();

                SqliteCommand selectCommand = new SqliteCommand
                    (sqlText, db);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    entries.Add(query.GetString(0));
                    entries.Add(query.GetString(1));
                }

                db.Close();
            }

            return entries;
        }

        private static string SelectTextForSignup()
        {
            return "SELECT " +
                nameof(Users.UserId) + "," +
                nameof(Users.Mobile) + " from " +
                nameof(Users);
        }

        private static string SelectTextForLogin()
        {
            return "SELECT " +
                nameof(LoginDetail.UserId) + ", " +
                nameof(LoginDetail.NumberOfLogin) + " from " +
                nameof(LoginDetail);
        }

        public static IDictionary<string, string> GetDataForLogin(string userName, string password)
        {
            IDictionary<string, string> entries = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                string emailOrMobile = userName.All(char.IsDigit)
                    ?
                    nameof(Users.Mobile)
                    :
                    nameof(Users.EmailId);

                GetDbPath();

                using (SqliteConnection db = new SqliteConnection(_fileName.FormatString(_dbpath)))
                {
                    db.Open();

                    SqliteCommand selectCommand = new SqliteCommand
                        ("SELECT * from " +
                        nameof(Users) + " Where " +
                        emailOrMobile + " = '" + userName + "' AND " +
                        nameof(Users.Password) + " = '" + password + "'", db);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        foreach (UsersEnum users in Enum.GetValues(typeof(UsersEnum)))
                        {
                            if(!users.ConvertToString().FormatStringEquals(nameof(Users.Id)) &&
                                !users.ConvertToString().FormatStringEquals(nameof(Users.Password)))
                                entries.Add(users.ConvertToString(), query.GetString(users.ToInt32()));
                        }
                    }

                    db.Close();
                }
            }

            return entries;
        }

        public static IDictionary<string, string> GetPreviousLoginDetail()
        {
            IDictionary<string, string> entries = new Dictionary<string, string>();
            InitializeDatabase(ApplicationConstants.Login);
            GetDbPath();

            using (SqliteConnection db = new SqliteConnection(_fileName.FormatString(_dbpath)))
            {
                db.Open();

                string sqlText = "SELECT * from " +
                    nameof(LoginDetail) +
                    " ORDER BY " +
                    nameof(LoginDetail.LastLogin) + " DESC ";

                SqliteCommand selectCommand = new SqliteCommand
                    (sqlText, db);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    entries.Add(nameof(LoginDetail.Id), query.GetString(0));
                    entries.Add(nameof(LoginDetail.UserName), query.GetString(1));
                    entries.Add(nameof(LoginDetail.UserId), query.GetString(2));
                    entries.Add(nameof(LoginDetail.IsRememberChecked), query.GetString(3));
                    entries.Add(nameof(LoginDetail.NumberOfLogin), query.GetString(4));
                    entries.Add(nameof(LoginDetail.LastLogin), query.GetString(5));

                    break;
                }

                db.Close();
            }

            return entries;
        }

        public static IDictionary<string, string> GetDataForOTP(string emailId, string mobileNo)
        {
            IDictionary<string, string> entries = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(emailId) && !string.IsNullOrEmpty(mobileNo))
            {
                GetDbPath();

                using (SqliteConnection db = new SqliteConnection(_fileName.FormatString(_dbpath)))
                {
                    db.Open();

                    SqliteCommand selectCommand = new SqliteCommand
                        ("SELECT * from " +
                        nameof(Users) + " Where " +
                        nameof(Users.EmailId) + " = '" + emailId + "' AND " +
                        nameof(Users.Mobile) + " = '" + mobileNo + "'", db);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        foreach (UsersEnum users in Enum.GetValues(typeof(UsersEnum)))
                        {
                            entries.Add(users.ConvertToString(), query.GetString(users.ToInt32()));
                        }
                    }

                    db.Close();
                }
            }

            return entries;
        }


    }
}
