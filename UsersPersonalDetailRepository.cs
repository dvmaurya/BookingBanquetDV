using BookingServices.Repository.Interface;
using CommonServices.Analytcis;
using CommonServices.Extensions;
using CommonServices.Logger;
using CommonServices.Model.DbModel;
using DataAccessLibrary;
using DataAccessLibrary.DML;
using Microsoft.Data.Sqlite;
using System;
using System.Linq;

namespace BookingServices.Repository
{
    public class UsersPersonalDetailRepository : BaseRepository, IUsersPersonalDetailRepository
    {
        private string _dbpath;
        private static string _fileName = "Filename={0}";

        public UsersPersonalDetailRepository(
            IAnalyticsManager analyticsManager) :
            base(analyticsManager)
        {

        }

        public bool AddOrUpdateUsersPersonalDetail(UsersPersonalDetail usersPersonalDetail)
        {
            try
            {
                string insertOrUpdate = string.Empty;
                _dbpath = DataAccess.GetDbPath();

                insertOrUpdate = usersPersonalDetail.Id == 0
                    ?
                    UsersPersonalDetailQuery.InsertInUsersPersonalDetail
                    :
                    UsersPersonalDetailQuery.UpdateInUsersPersonalDetail;

                using (SqliteConnection db = new SqliteConnection(_fileName.FormatString(_dbpath)))
                {
                    db.Open();

                    using (SqliteCommand insertCommand = new SqliteCommand(insertOrUpdate, db))
                    {
                        insertCommand.Parameters.AddWithValue("@" + nameof(usersPersonalDetail.FullName), usersPersonalDetail.FullName);
                        insertCommand.Parameters.AddWithValue("@" + nameof(usersPersonalDetail.EmailId), usersPersonalDetail.EmailId);
                        insertCommand.Parameters.AddWithValue("@" + nameof(usersPersonalDetail.Mobile), usersPersonalDetail.Mobile);
                        insertCommand.Parameters.AddWithValue("@" + nameof(usersPersonalDetail.Address), usersPersonalDetail.Address);
                        insertCommand.Parameters.AddWithValue("@" + nameof(usersPersonalDetail.Country), usersPersonalDetail.Country);
                        insertCommand.Parameters.AddWithValue("@" + nameof(usersPersonalDetail.State), usersPersonalDetail.State);
                        insertCommand.Parameters.AddWithValue("@" + nameof(usersPersonalDetail.City), usersPersonalDetail.City);
                        insertCommand.Parameters.AddWithValue("@" + nameof(usersPersonalDetail.CreationDate), usersPersonalDetail.CreationDate);

                        insertCommand.ExecuteReader();
                    }

                    db.Close();

                    TraceEvents(
                        analyticsEvent: AnalyticConstants.Application,
                        action: $"{AnalyticConstants.User} " +
                                $"{AnalyticConstants.Personal} " +
                                $"{AnalyticConstants.Detail} ",
                        label: $"{AnalyticConstants.User} " +
                                $"{AnalyticConstants.Personal} " +
                                $"{AnalyticConstants.Detail} " +
                                $"{AnalyticConstants.Added} " +
                                $"{AnalyticConstants.Successfully} " +
                                $"in {AnalyticConstants.Database} ");

                    return true;
                }
            }
            catch (Exception ex)
            {
                FileLogger.Logger.Info("User personal detail Exception: ".ConcatString(ex.Message));
                TraceExceptions("User personal detail exception: ".ConcatString(ex.Message));

                return false;
            }
        }

        public UsersPersonalDetail GetUsersPersonalDetail(string userId)
        {
            UsersPersonalDetail usersPersonalDetail = null;

            try
            {
                if (!string.IsNullOrEmpty(userId))
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

                        using (SqliteCommand selectCommand = new SqliteCommand(
                            UsersPersonalDetailQuery.GetUsersPersonalDetailByUserId.FormatString(emailOrMobile, userId),
                            db))
                        {
                            using (SqliteDataReader query = selectCommand.ExecuteReader())
                            {
                                while (query.Read())
                                {
                                    usersPersonalDetail = new UsersPersonalDetail
                                    {
                                        Id = query.GetString(0).ToInt32(),
                                        FullName = query.GetString(1),
                                        EmailId = query.GetString(2),
                                        Mobile = query.GetString(3),
                                        Address = query.GetString(4),
                                        Country = query.GetString(5),
                                        State = query.GetString(6),
                                        City = query.GetString(7),
                                        CreationDate = query.GetString(8)
                                    };
                                }
                            }
                        }

                        db.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogger.Logger.Info("User personal detail Exception: ".ConcatString(ex.Message));
                TraceExceptions("Get user personal detail from database exception: ".ConcatString(ex.Message));
            }

            return usersPersonalDetail;
        }
    }
}
