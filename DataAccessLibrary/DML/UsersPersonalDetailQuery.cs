using CommonServices.Model.DbModel;

namespace DataAccessLibrary.DML
{
    public static class UsersPersonalDetailQuery
    {
        public const string CreateUsersPersonalDetailTable =
            "CREATE TABLE IF NOT " +
            "EXISTS " + nameof(UsersPersonalDetail) +
            " ( " + nameof(UsersPersonalDetail.Id) + " INTEGER PRIMARY KEY, " +
            " " + nameof(UsersPersonalDetail.FullName) + " NVARCHAR(2048) NUll," +
            " " + nameof(UsersPersonalDetail.EmailId) + " NVARCHAR(2048) NUll," +
            " " + nameof(UsersPersonalDetail.Mobile) + " NVARCHAR(2048) NUll," +
            " " + nameof(UsersPersonalDetail.Address) + " NVARCHAR(2048) NULL, " +
            " " + nameof(UsersPersonalDetail.Country) + " NVARCHAR(2048) NULL, " +
            " " + nameof(UsersPersonalDetail.State) + " NVARCHAR(2048) NULL, " +
            " " + nameof(UsersPersonalDetail.City) + " NVARCHAR(2048) NULL)";

        public const string InsertInUsersPersonalDetail =
            "INSERT INTO " + nameof(UsersPersonalDetail) + " VALUES (" +
            "NULL, " +
            "@" + nameof(UsersPersonalDetail.FullName) + "," +
            "@" + nameof(UsersPersonalDetail.EmailId) + "," +
            "@" + nameof(UsersPersonalDetail.Mobile) + "," +
            "@" + nameof(UsersPersonalDetail.Address) + "," +
            "@" + nameof(UsersPersonalDetail.Country) + "," +
            "@" + nameof(UsersPersonalDetail.State) + "," +
            "@" + nameof(UsersPersonalDetail.City) + ");";

        public const string UpdateInUsersPersonalDetail =
            "UPDATE " +
            nameof(UsersPersonalDetail) + " SET " +
            nameof(UsersPersonalDetail.FullName) + " = @" + nameof(UsersPersonalDetail.FullName) + ", " +
            nameof(UsersPersonalDetail.EmailId) + " = @" + nameof(UsersPersonalDetail.EmailId) + ", " +
            nameof(UsersPersonalDetail.Mobile) + " = @" + nameof(UsersPersonalDetail.Mobile) + ", " +
            nameof(UsersPersonalDetail.Address) + " = @" + nameof(UsersPersonalDetail.Address) + ", " +
            nameof(UsersPersonalDetail.Country) + " = @" + nameof(UsersPersonalDetail.Country) + ", " +
            nameof(UsersPersonalDetail.State) + " = @" + nameof(UsersPersonalDetail.State) + ", " +
            nameof(UsersPersonalDetail.City) + " = @" + nameof(UsersPersonalDetail.City) + " Where " +
            nameof(UsersPersonalDetail.EmailId) + " = @" + nameof(UsersPersonalDetail.EmailId);

        public const string GetUsersPersonalDetailByUserId =
            "SELECT " +
            nameof(UsersPersonalDetail.Id) + "," +
            nameof(UsersPersonalDetail.FullName) + "," +
            nameof(UsersPersonalDetail.EmailId) + "," +
            nameof(UsersPersonalDetail.Mobile) + "," +
            nameof(UsersPersonalDetail.Address) + "," +
            nameof(UsersPersonalDetail.Country) + "," +
            nameof(UsersPersonalDetail.State) + "," +
            nameof(UsersPersonalDetail.City) + " from " +
            nameof(UsersPersonalDetail) + " Where {0} = '{1}'";
    }
}
