using BookingServices.Repository.Interface;
using CommonServices.Enums;
using CommonServices.Extensions;
using CommonServices.Logger;
using CommonServices.Model;
using DataAccessLibrary;
using DataAccessLibrary.DML;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace BookingServices.Repository
{
    public class UserRepository : IUserRepository
    {
        private string _dbpath;
        private static string _fileName = "Filename={0}";

        public bool AddOrUpdateUsers(Users users)
        {
            try
            {
                string insertOrUpdate = string.Empty;
                _dbpath = DataAccess.GetDbPath();

                insertOrUpdate = users.Id == 0
                    ?
                    SignupQuery.InsertInSignup
                    :
                    SignupQuery.UpdateInSignup;

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
                    insertCommand.Parameters.AddWithValue("@" + nameof(users.IsActivate), users.IsActivate);
                    insertCommand.Parameters.AddWithValue("@" + nameof(users.PasswordChanged), users.PasswordChanged);

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

        public List<string> GetAllUsers()
        {
            List<string> entries = new List<string>();
            _dbpath = DataAccess.GetDbPath();

            using (SqliteConnection db = new SqliteConnection(_fileName.FormatString(_dbpath)))
            {
                db.Open();

                string sqlText = string.Empty;

                sqlText = SignupQuery.SelectUserIdFromUser;

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

        public IDictionary<string, string> GetDataForOTP(string emailId, string mobileNo)
        {
            IDictionary<string, string> entries = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(emailId) && !string.IsNullOrEmpty(mobileNo))
            {
                _dbpath = DataAccess.GetDbPath();

                using (SqliteConnection db = new SqliteConnection(_fileName.FormatString(_dbpath)))
                {
                    db.Open();

                    SqliteCommand selectCommand = new SqliteCommand
                        (SignupQuery.SelectAllFromUserWithEmailAndMobile.
                        FormatString(emailId, mobileNo), db);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        foreach (UsersEnum users in Enum.GetValues(typeof(UsersEnum)))
                            entries.Add(users.ConvertToString(), query.GetString(users.ToInt32()));
                    }

                    db.Close();
                }
            }

            return entries;
        }
    }
}
