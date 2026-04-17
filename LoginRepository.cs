using BookingServices.Repository.Interface;
using CommonServices.Analytcis;
using CommonServices.Extensions;
using CommonServices.Logger;
using CommonServices.Model.DbModel;
using CommonServices.Utilities;
using DataAccessLibrary;
using DataAccessLibrary.DML;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookingServices.Repository
{
    public class LoginRepository : BaseRepository, ILoginRepository
    {
        private string _dbpath;
        private static string _fileName = "Filename={0}";

        public LoginRepository(
            IAnalyticsManager analyticManager) :
            base(analyticManager)
        {

        }

        public bool AddOrUpdateLoginDetail(LoginDetail loginDetail)
        {
            string insertOrUpdate = string.Empty;
            try
            {
                _dbpath = DataAccess.GetDbPath();

                IDictionary<string, string> userExist = GetLoginDetails(loginDetail);

                if (userExist != null && userExist.Any())
                {
                    loginDetail.NumberOfLogin = (userExist[nameof(LoginDetail.NumberOfLogin)].ToInt32() + 1).ConvertToString();
                    insertOrUpdate = LoginQuery.UpdateLoginDetail;
                }
                else
                {
                    insertOrUpdate = LoginQuery.InsertInLogin;
                }

                using (SqliteConnection db = new SqliteConnection(_fileName.FormatString(_dbpath)))
                {
                    db.Open();
                    using (SqliteCommand insertCommand = new SqliteCommand(insertOrUpdate, db))
                    {
                        insertCommand.Parameters.AddWithValue("@" + nameof(loginDetail.MobileNo), loginDetail.MobileNo);
                        insertCommand.Parameters.AddWithValue("@" + nameof(loginDetail.EmailId), loginDetail.EmailId);
                        insertCommand.Parameters.AddWithValue("@" + nameof(loginDetail.IsRememberChecked), loginDetail.IsRememberChecked);
                        insertCommand.Parameters.AddWithValue("@" + nameof(loginDetail.NumberOfLogin), loginDetail.NumberOfLogin);
                        insertCommand.Parameters.AddWithValue("@" + nameof(loginDetail.LastLogin), loginDetail.LastLogin);

                        insertCommand.ExecuteReader();
                    }

                    db.Close();

                    TraceEvents(
                        analyticsEvent: AnalyticConstants.Application,
                        action: $"{AnalyticConstants.Login} " +
                                $"{AnalyticConstants.Deleted} ",
                        label: $"{AnalyticConstants.Login} " +
                                $"{AnalyticConstants.Deleted} " +
                                $"{AnalyticConstants.Added} " +
                                $"in {AnalyticConstants.Database} " +
                                $"{AnalyticConstants.Successfully} ");

                    return true;
                }
            }
            catch (Exception ex)
            {
                FileLogger.Logger.Info("Add login detail into database exception: ".ConcatString(ex.Message));
                TraceExceptions("Add login detail into database exception: ".ConcatString(ex.Message));

                return false;
            }
        }

        private IDictionary<string, string> GetLoginDetails(LoginDetail loginDetail)
        {
            IDictionary<string, string> entries = new Dictionary<string, string>();
            _dbpath = DataAccess.GetDbPath();

            try
            {
                using (SqliteConnection db = new SqliteConnection(_fileName.FormatString(_dbpath)))
                {
                    string sqlText = LoginQuery.SelectAllFromLogin.FormatString(loginDetail.MobileNo);

                    db.Open();
                    using (SqliteCommand selectCommand = new SqliteCommand(sqlText, db))
                    {
                        using (SqliteDataReader query = selectCommand.ExecuteReader())
                        {
                            while (query.Read())
                            {
                                entries.Add(nameof(LoginDetail.Id), query.GetString(0));
                                entries.Add(nameof(LoginDetail.MobileNo), query.GetString(1));
                                entries.Add(nameof(LoginDetail.EmailId), query.GetString(2));
                                entries.Add(nameof(LoginDetail.IsRememberChecked), query.GetString(3));
                                entries.Add(nameof(LoginDetail.NumberOfLogin), query.GetString(4));
                                entries.Add(nameof(LoginDetail.LastLogin), query.GetString(5));
                            }
                        }
                    }

                    db.Close();
                }
            }
            catch (Exception ex)
            {
                FileLogger.Logger.Info("Get login detail exception: " + ex.Message);
                TraceExceptions("Get login detail exception: ".ConcatString(ex.Message));
            }

            return entries;
        }

        public IDictionary<string, string> GetDataForLogin(string userName, string password)
        {
            IDictionary<string, string> entries = new Dictionary<string, string>();

            try
            {
                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
                {
                    string selectQuery = string.Empty;
                    string sha1Password = Utilities.GetSha1Hashed(userName + "|" + password);

                    selectQuery = userName.All(char.IsDigit) ?
                        SignupQuery.SelectAllFromUserMobile.FormatString(
                            nameof(UsersCred.Mobile), userName, sha1Password) :
                        SignupQuery.SelectAllFromUserEmail.FormatString(
                            nameof(UsersCred.EmailId), userName, sha1Password);

                    _dbpath = DataAccess.GetDbPath();

                    using (SqliteConnection db = new SqliteConnection(_fileName.FormatString(_dbpath)))
                    {
                        db.Open();

                        using (SqliteCommand selectCommand = new SqliteCommand(selectQuery, db))
                        {
                            using (SqliteDataReader executeQuery = selectCommand.ExecuteReader())
                            {
                                while (executeQuery.Read())
                                {
                                    entries.Add(nameof(UsersCred.Id), executeQuery.GetString(0));
                                    entries.Add(nameof(UsersCred.UserId), executeQuery.GetString(1));
                                    entries.Add(nameof(UsersCred.EmailId), executeQuery.GetString(2));
                                    entries.Add(nameof(UsersCred.Mobile), executeQuery.GetString(3));
                                    entries.Add(nameof(UsersCred.MobilePassword), executeQuery.GetString(4));
                                    entries.Add(nameof(UsersCred.EmailPassword), executeQuery.GetString(5));
                                    entries.Add(nameof(UsersCred.CreationDate), executeQuery.GetString(6));
                                    entries.Add(nameof(UsersCred.IsAdmin), executeQuery.GetString(7));
                                    entries.Add(nameof(UsersCred.PasswordChanged), executeQuery.GetString(8));
                                }
                            }
                        }

                        db.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogger.Logger.Info("Get login detail from database exception: " + ex.Message);
                TraceExceptions("Get login detail from database exception: ".ConcatString(ex.Message));
            }

            return entries;
        }

        public IDictionary<string, string> GetPreviousLoginDetail()
        {
            IDictionary<string, string> entries = new Dictionary<string, string>();
            _dbpath = DataAccess.GetDbPath();

            try
            {
                using (SqliteConnection db = new SqliteConnection(_fileName.FormatString(_dbpath)))
                {
                    db.Open();

                    using (SqliteCommand selectCommand = new SqliteCommand
                        (LoginQuery.SelectAllFromLoginInDec, db))
                    {
                        using (SqliteDataReader query = selectCommand.ExecuteReader())
                        {
                            while (query.Read())
                            {
                                entries.Add(nameof(LoginDetail.Id), query.GetString(0));
                                entries.Add(nameof(LoginDetail.MobileNo), query.GetString(1));
                                entries.Add(nameof(LoginDetail.IsRememberChecked), query.GetString(2));
                                entries.Add(nameof(LoginDetail.NumberOfLogin), query.GetString(3));
                                entries.Add(nameof(LoginDetail.LastLogin), query.GetString(4));

                                break;
                            }
                        }
                    }

                    db.Close();
                }
            }
            catch (Exception ex)
            {
                FileLogger.Logger.Info("Get previous login detail exception: " + ex.Message);
                TraceExceptions("Get previous login detail exception: ".ConcatString(ex.Message));
            }

            return entries;
        }
    }
}
