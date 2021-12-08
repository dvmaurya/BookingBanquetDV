using BookingServices.Repository.Interface;
using CommonServices.Enums;
using CommonServices.Extensions;
using CommonServices.Logger;
using CommonServices.Model.DbModel;
using DataAccessLibrary;
using DataAccessLibrary.DML;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookingServices.Repository
{
    public class UserCredRepository : IUserCredRepository
    {
        private string _dbpath;
        private static string _fileName = "Filename={0}";

        public bool AddOrUpdateUsersCred(UsersCred usersCred)
        {
            try
            {
                _dbpath = DataAccess.GetDbPath();

                string insertOrUpdate = usersCred.Id == 0
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

                    insertCommand.Parameters.AddWithValue("@" + nameof(usersCred.UserId), usersCred.UserId);
                    insertCommand.Parameters.AddWithValue("@" + nameof(usersCred.FullName), usersCred.FullName);
                    insertCommand.Parameters.AddWithValue("@" + nameof(usersCred.EmailId), usersCred.EmailId);
                    insertCommand.Parameters.AddWithValue("@" + nameof(usersCred.Mobile), usersCred.Mobile);
                    insertCommand.Parameters.AddWithValue("@" + nameof(usersCred.Password), usersCred.Password);
                    insertCommand.Parameters.AddWithValue("@" + nameof(usersCred.CreationDate), usersCred.CreationDate);
                    insertCommand.Parameters.AddWithValue("@" + nameof(usersCred.IsAdmin), usersCred.IsAdmin);
                    insertCommand.Parameters.AddWithValue("@" + nameof(usersCred.PasswordChanged), usersCred.PasswordChanged);
                    
                    insertCommand.ExecuteReader();
                    db.Close();

                    return true;
                }
            }
            catch (Exception ex)
            {
                FileLogger.Logger.Info("User Exception: ".ConcatString(ex.Message));
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

        public IDictionary<string, string> GetUserDetails(string emailId, string mobileNo)
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

        public UsersCred GetUsersDetail(string userId)
        {
            UsersCred users = new UsersCred();

            if (userId != null)
            {
                string emailOrMobile = userId.All(char.IsDigit)
                    ?
                    nameof(UsersCred.Mobile)
                    :
                    nameof(UsersCred.EmailId);

                _dbpath = DataAccess.GetDbPath();

                using (SqliteConnection db = new SqliteConnection(_fileName.FormatString(_dbpath)))
                {
                    db.Open();

                    SqliteCommand selectCommand = new SqliteCommand
                        (SignupQuery.GetUserDetailByUserId.
                        FormatString(emailOrMobile, userId), db);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        users.UserId = query.GetString(0);
                        users.FullName = query.GetString(1);
                        users.EmailId = query.GetString(2);
                        users.Mobile = query.GetString(3);
                        users.CreationDate = query.GetString(4);
                    }

                    db.Close();
                }
            }

            return users;
        }
    }
}
