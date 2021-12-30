using BookingServices.Repository.Interface;
using CommonServices.Extensions;
using CommonServices.Logger;
using CommonServices.Model.DbModel;
using DataAccessLibrary;
using DataAccessLibrary.DML;
using Microsoft.Data.Sqlite;
using System;

namespace BookingServices.Repository
{
    public class BanquetRepository : IBanquetRepository
    {
        private string _dbpath;
        private static string _fileName = "Filename={0}";

        public void AddOrUpdateBanquet(BanquetDetail banquetDetail)
        {
            string InsertOrUpdate = string.Empty;
            try
            {
                _dbpath = DataAccess.GetDbPath();

                InsertOrUpdate = banquetDetail != null && banquetDetail.Id != 0 ?
                    BanquetDetailQuery.UpdateBanquetDetail : 
                    BanquetDetailQuery.InsertInBanquetDetail;

                using (SqliteConnection db = new SqliteConnection(_fileName.FormatString(_dbpath)))
                {
                    db.Open();

                    SqliteCommand insertCommand = new SqliteCommand
                    {
                        Connection = db,
                        CommandText = InsertOrUpdate
                    };

                    insertCommand.Parameters.AddWithValue("@" + nameof(banquetDetail.Id), banquetDetail.Id);
                    insertCommand.Parameters.AddWithValue("@" + nameof(banquetDetail.AvailableFlours), banquetDetail.AvailableFlours);
                    insertCommand.Parameters.AddWithValue("@" + nameof(banquetDetail.AvailableMandap), banquetDetail.AvailableMandap);
                    insertCommand.Parameters.AddWithValue("@" + nameof(banquetDetail.AvailableParkings), banquetDetail.AvailableParkings);
                    insertCommand.Parameters.AddWithValue("@" + nameof(banquetDetail.AvailableRooms), banquetDetail.AvailableRooms);
                    insertCommand.Parameters.AddWithValue("@" + nameof(banquetDetail.EmailId), banquetDetail.EmailId);
                    insertCommand.Parameters.AddWithValue("@" + nameof(banquetDetail.UpdationDate), banquetDetail.UpdationDate);

                    insertCommand.ExecuteReader();

                    db.Close();
                }
            }
            catch (Exception ex)
            {
                FileLogger.Logger.Info("Bnquet add Exception: ".ConcatString(ex.Message));
            }
        }

        public BanquetDetail GetBanquetDetail()
        {
            _dbpath = DataAccess.GetDbPath();

            using (SqliteConnection db = new SqliteConnection(_fileName.FormatString(_dbpath)))
            {
                db.Open();

                string sqlText = BanquetDetailQuery.GetBanquetDetail;

                SqliteCommand selectCommand = new SqliteCommand
                    (sqlText, db);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    return new BanquetDetail
                    {
                        Id = query.GetString(0).ToInt32(),
                        AvailableFlours = query.GetString(1),
                        AvailableMandap = query.GetString(2),
                        AvailableParkings = query.GetString(3),
                        AvailableRooms = query.GetString(4),
                        EmailId = query.GetString(5),
                        UpdationDate = query.GetString(6)
                    };
                }

                db.Close();
            }

            return null;
        }
    }
}
