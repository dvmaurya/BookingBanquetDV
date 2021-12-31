using BookingServices.Repository.Interface;
using CommonServices.Analytcis;
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
    public class LoginRepository : BaseRepository, ILoginRepository
    {
        private string _dbpath;
        private static string _fileName = "Filename={0}";

        public LoginRepository(
            IAnalyticsManager analyticManager):
            base(analyticManager)
        {

        }

        public bool AddOrUpdateLoginDetail(LoginDetail loginDetail)
        {
            string InsertOrUpdate = string.Empty;
            try
            {
                _dbpath = DataAccess.GetDbPath();

                IDictionary<string, string> userExist = GetLoginDetails(loginDetail);

                if (userExist != null && userExist.Any())
                {
                    loginDetail.NumberOfLogin = (userExist[nameof(LoginDetail.NumberOfLogin)].ToInt32() + 1).ConvertToString();
                    InsertOrUpdate = LoginQuery.UpdateLoginDetail;
                }
                else
                {
                    InsertOrUpdate = LoginQuery.InsertInLogin;
                }

                using (SqliteConnection db = new SqliteConnection(_fileName.FormatString(_dbpath)))
                {
                    db.Open();

                    SqliteCommand insertCommand = new SqliteCommand
                    {
                        Connection = db,
                        CommandText = InsertOrUpdate
                    };

                    insertCommand.Parameters.AddWithValue("@" + nameof(loginDetail.UserId), loginDetail.UserId);
                    insertCommand.Parameters.AddWithValue("@" + nameof(loginDetail.EmailId), loginDetail.EmailId);
                    insertCommand.Parameters.AddWithValue("@" + nameof(loginDetail.IsRememberChecked), loginDetail.IsRememberChecked);
                    insertCommand.Parameters.AddWithValue("@" + nameof(loginDetail.NumberOfLogin), loginDetail.NumberOfLogin);
                    insertCommand.Parameters.AddWithValue("@" + nameof(loginDetail.LastLogin), loginDetail.LastLogin);

                    insertCommand.ExecuteReader();

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
                FileLogger.Logger.Info("Exception: ".ConcatString(ex.Message));
                TraceEvents(
                    analyticsEvent: AnalyticConstants.Exception,
                    action: $"{AnalyticConstants.Login} " +
                            $"{AnalyticConstants.Detail} ",
                    label: $"{AnalyticConstants.Login} " +
                            $"{AnalyticConstants.Detail} " +
                            $"{AnalyticConstants.Added} " +
                            $"in {AnalyticConstants.Database} " +
                            $"{AnalyticConstants.Failed} ");

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
                    db.Open();

                    string sqlText = LoginQuery.SelectAllFromLogin.FormatString(loginDetail.UserId);

                    SqliteCommand selectCommand = new SqliteCommand
                        (sqlText, db);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        entries.Add(nameof(LoginDetail.Id), query.GetString(0));
                        entries.Add(nameof(LoginDetail.UserId), query.GetString(1));
                        entries.Add(nameof(LoginDetail.EmailId), query.GetString(2));
                        entries.Add(nameof(LoginDetail.IsRememberChecked), query.GetString(3));
                        entries.Add(nameof(LoginDetail.NumberOfLogin), query.GetString(4));
                        entries.Add(nameof(LoginDetail.LastLogin), query.GetString(5));
                    }

                    db.Close();
                }
            }
            catch(Exception ex)
            {
                TraceEvents(
                    analyticsEvent: AnalyticConstants.Exception,
                    action: $"{AnalyticConstants.Get} " +
                            $"{AnalyticConstants.Login} " +
                            $"{AnalyticConstants.Detail} ",
                    label: $"{AnalyticConstants.Get} " +
                            $"{AnalyticConstants.Login} " +
                            $"{AnalyticConstants.Detail} " +
                            $"from {AnalyticConstants.Database} " +
                            $"{AnalyticConstants.Failed} " +
                            $"{ex.Message}");
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
                    string emailOrMobile = userName.All(char.IsDigit)
                        ?
                        nameof(UsersCred.Mobile)
                        :
                        nameof(UsersCred.EmailId);

                    _dbpath = DataAccess.GetDbPath();

                    using (SqliteConnection db = new SqliteConnection(_fileName.FormatString(_dbpath)))
                    {
                        db.Open();

                        SqliteCommand selectCommand = new SqliteCommand
                            (SignupQuery.SelectAllFromUser.FormatString(
                                emailOrMobile, userName, password),
                            db);

                        SqliteDataReader query = selectCommand.ExecuteReader();

                        while (query.Read())
                        {
                            entries.Add(nameof(UsersCred.Id), query.GetString(0));
                            entries.Add(nameof(UsersCred.UserId), query.GetString(1));
                            entries.Add(nameof(UsersCred.EmailId), query.GetString(2));
                            entries.Add(nameof(UsersCred.Mobile), query.GetString(3));
                            entries.Add(nameof(UsersCred.Password), query.GetString(4));
                            entries.Add(nameof(UsersCred.CreationDate), query.GetString(5));
                            entries.Add(nameof(UsersCred.IsAdmin), query.GetString(6));
                            entries.Add(nameof(UsersCred.PasswordChanged), query.GetString(7));
                        }

                        db.Close();
                    }
                }
            }
            catch(Exception ex)
            {
                TraceEvents(
                    analyticsEvent: AnalyticConstants.Exception,
                    action: $"{AnalyticConstants.Get} " +
                            $"{AnalyticConstants.Login} " +
                            $"{AnalyticConstants.Data} ",
                    label: $"{AnalyticConstants.Get} " +
                            $"{AnalyticConstants.Login} " +
                            $"{AnalyticConstants.Data} " +
                            $"for {AnalyticConstants.Login} " +
                            $"from {AnalyticConstants.Database} " +
                            $"{AnalyticConstants.Failed} " +
                            $"{ex.Message}");
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

                    SqliteCommand selectCommand = new SqliteCommand
                        (LoginQuery.SelectAllFromLoginInDec, db);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        entries.Add(nameof(LoginDetail.Id), query.GetString(0));
                        entries.Add(nameof(LoginDetail.UserId), query.GetString(1));
                        entries.Add(nameof(LoginDetail.IsRememberChecked), query.GetString(2));
                        entries.Add(nameof(LoginDetail.NumberOfLogin), query.GetString(3));
                        entries.Add(nameof(LoginDetail.LastLogin), query.GetString(4));

                        break;
                    }

                    db.Close();
                }
            }
            catch(Exception ex)
            {
                TraceEvents(
                    analyticsEvent: AnalyticConstants.Exception,
                    action: $"{AnalyticConstants.Get} " +
                            $"{AnalyticConstants.Previous} " +
                            $"{AnalyticConstants.Login} " +
                            $"{AnalyticConstants.Detail} ",
                    label: $"{AnalyticConstants.Get} " +
                            $"{AnalyticConstants.Previous} " +
                            $"{AnalyticConstants.Login} " +
                            $"{AnalyticConstants.Detail} " +
                            $"from {AnalyticConstants.Database} " +
                            $"{AnalyticConstants.Failed} " +
                            $"{ex.Message}");
            }

            return entries;
        }
    }
}
