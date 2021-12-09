using CommonServices.Model.DbModel;

namespace DataAccessLibrary.DML
{
    public static class LoginQuery
    {
        public const string CreateLoginTable =
            "CREATE TABLE IF NOT " +
            "EXISTS " + nameof(LoginDetail) +
            " ( " + nameof(LoginDetail.Id) + " INTEGER PRIMARY KEY, " +
            " " + nameof(LoginDetail.UserId) + " NVARCHAR(2048) NUll," +
            " " + nameof(LoginDetail.EmailId) + " NVARCHAR(2048) NUll," +
            " " + nameof(LoginDetail.IsRememberChecked) + " NVARCHAR(2048) NUll," +
            " " + nameof(LoginDetail.NumberOfLogin) + " NVARCHAR(2048) NUll," +
            " " + nameof(LoginDetail.LastLogin) + " NVARCHAR(2048) NUll)";

        public const string UpdateLoginDetail =
            "UPDATE " +
            nameof(LoginDetail) + " SET " +
            nameof(LoginDetail.UserId) + " = @" + nameof(LoginDetail.UserId) + ", " +
            nameof(LoginDetail.EmailId) + " = @" + nameof(LoginDetail.UserId) + ", " +
            nameof(LoginDetail.IsRememberChecked) + " = @" + nameof(LoginDetail.IsRememberChecked) + ", " +
            nameof(LoginDetail.NumberOfLogin) + " = @" + nameof(LoginDetail.NumberOfLogin) + ", " +
            nameof(LoginDetail.LastLogin) + " = @" + nameof(LoginDetail.LastLogin) + " Where " +
            nameof(LoginDetail.UserId) + " = @" + nameof(LoginDetail.UserId);

        public const string InsertInLogin =
            "INSERT INTO " + nameof(LoginDetail) + " VALUES (" +
            "NULL, " +
            "@" + nameof(LoginDetail.UserId) + "," +
            "@" + nameof(LoginDetail.EmailId) + "," +
            "@" + nameof(LoginDetail.IsRememberChecked) + "," +
            "@" + nameof(LoginDetail.NumberOfLogin) + "," +
            "@" + nameof(LoginDetail.LastLogin) + ");";

        public const string SelectAllFromLogin =
            "SELECT * from " +
            nameof(LoginDetail) + " Where " +
            nameof(LoginDetail.UserId) + " = '{0}'";

        public const string SelectAllFromLoginInDec =
            "SELECT * from " +
            nameof(LoginDetail) +
            " ORDER BY " +
            nameof(LoginDetail.LastLogin) + " DESC ";
    }
}
