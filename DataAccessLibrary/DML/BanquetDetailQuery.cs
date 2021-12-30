using CommonServices.Model.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.DML
{
    public class BanquetDetailQuery
    {
        public const string CreateBanquetDetailTable =
            "CREATE TABLE IF NOT " +
            "EXISTS " + nameof(BanquetDetail) +
            " ( " + nameof(BanquetDetail.Id) + " INTEGER PRIMARY KEY, " +
            " " + nameof(BanquetDetail.AvailableFlours) + " NVARCHAR(2048) NUll," +
            " " + nameof(BanquetDetail.AvailableMandap) + " NVARCHAR(2048) NUll," +
            " " + nameof(BanquetDetail.AvailableParkings) + " NVARCHAR(2048) NUll," +
            " " + nameof(BanquetDetail.AvailableRooms) + " NVARCHAR(2048) NUll," +
            " " + nameof(BanquetDetail.EmailId) + " NVARCHAR(2048) NUll," +
            " " + nameof(BanquetDetail.UpdationDate) + " NVARCHAR(2048) NUll)";

        public const string UpdateBanquetDetail =
            "UPDATE " +
            nameof(BanquetDetail) + " SET " +
            nameof(BanquetDetail.AvailableFlours) + " = @" + nameof(BanquetDetail.AvailableFlours) + ", " +
            nameof(BanquetDetail.AvailableMandap) + " = @" + nameof(BanquetDetail.AvailableMandap) + ", " +
            nameof(BanquetDetail.AvailableParkings) + " = @" + nameof(BanquetDetail.AvailableParkings) + ", " +
            nameof(BanquetDetail.AvailableRooms) + " = @" + nameof(BanquetDetail.AvailableRooms) + ", " +
            nameof(BanquetDetail.EmailId) + " = @" + nameof(BanquetDetail.EmailId) + ", " +
            nameof(BanquetDetail.UpdationDate) + " = @" + nameof(BanquetDetail.UpdationDate) + " Where " +
            nameof(BanquetDetail.Id) + " = @" + nameof(BanquetDetail.Id);

        public const string InsertInBanquetDetail =
           "INSERT INTO " + nameof(BanquetDetail) + " VALUES (" +
           "NULL, " +
           "@" + nameof(BanquetDetail.AvailableFlours) + "," +
           "@" + nameof(BanquetDetail.AvailableMandap) + "," +
           "@" + nameof(BanquetDetail.AvailableParkings) + "," +
           "@" + nameof(BanquetDetail.AvailableRooms) + "," +
           "@" + nameof(BanquetDetail.EmailId) + "," +
           "@" + nameof(BanquetDetail.UpdationDate) + ");";

        public const string GetBanquetDetail =
            "SELECT * from " + nameof(BanquetDetail);
    }
}
