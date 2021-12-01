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
using System.Linq;

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
                    insertCommand.Parameters.AddWithValue("@" + nameof(users.PhoneCode), users.PhoneCode);
                    insertCommand.Parameters.AddWithValue("@" + nameof(users.Address), users.Address);
                    insertCommand.Parameters.AddWithValue("@" + nameof(users.Country), users.Country);
                    insertCommand.Parameters.AddWithValue("@" + nameof(users.State), users.State);
                    insertCommand.Parameters.AddWithValue("@" + nameof(users.City), users.City);
                    insertCommand.Parameters.AddWithValue("@" + nameof(users.PhotoName), users.PhotoName);

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

        public Users GetUsersDetail(string userId)
        {
            Users users = new Users();

            if (userId != null)
            {
                string emailOrMobile = userId.All(char.IsDigit)
                    ?
                    nameof(Users.Mobile)
                    :
                    nameof(Users.EmailId);

                _dbpath = DataAccess.GetDbPath();

                using (SqliteConnection db = new SqliteConnection(_fileName.FormatString(_dbpath)))
                {
                    db.Open();

                    SqliteCommand selectCommand = new SqliteCommand
                        (SignupQuery.GetUserDetailBuUserId.
                        FormatString(emailOrMobile, userId), db);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        users.UserId = query.GetString(0);
                        users.FullName = query.GetString(1);
                        users.EmailId = query.GetString(2);
                        users.Mobile = query.GetString(3);
                        users.CreationDate = query.GetString(4);
                        users.PhoneCode = query.GetString(5);
                        users.Address = query.GetString(6);
                        users.Country = query.GetString(7);
                        users.State = query.GetString(8);
                        users.City = query.GetString(9);
                        users.PhotoName = query.GetString(10);
                    }

                    db.Close();
                }
            }

            return users;
        }
    }
}
