using CommonServices.Model;

namespace DataAccessLibrary.DML
{
    public static class SignupQuery
    {
        public const string CreateSignupTable =
            "CREATE TABLE IF NOT " +
            "EXISTS " + nameof(Users) +
            " ( " + nameof(Users.Id) + " INTEGER PRIMARY KEY, " +
            " " + nameof(Users.UserId) + " NVARCHAR(2048) NUll," +
            " " + nameof(Users.FullName) + " NVARCHAR(2048) NUll," +
            " " + nameof(Users.EmailId) + " NVARCHAR(2048) NUll," +
            " " + nameof(Users.Mobile) + " NVARCHAR(2048) NUll," +
            " " + nameof(Users.Password) + " NVARCHAR(2048) NUll," +
            " " + nameof(Users.CreationDate) + " NVARCHAR(2048) NULL, " +
            " " + nameof(Users.IsActivate) + " NVARCHAR(2048) NULL, " +
            " " + nameof(Users.PasswordChanged) + " NVARCHAR(2048) NULL)";

        public const string InsertInSignup = 
            "INSERT INTO " + nameof(Users) + " VALUES (" +
            "NULL, " +
            "@" + nameof(Users.UserId) + "," +
            "@" + nameof(Users.FullName) + "," +
            "@" + nameof(Users.EmailId) + "," +
            "@" + nameof(Users.Mobile) + "," +
            "@" + nameof(Users.Password) + "," +
            "@" + nameof(Users.CreationDate) + "," +
            "0, 0);";

        public const string UpdateInSignup = 
            "UPDATE " +
            nameof(Users) + " SET " +
            nameof(Users.UserId) + " = @" + nameof(Users.UserId) + ", " +
            nameof(Users.FullName) + " = @" + nameof(Users.FullName) + ", " +
            nameof(Users.EmailId) + " = @" + nameof(Users.EmailId) + ", " +
            nameof(Users.Mobile) + " = @" + nameof(Users.Mobile) + ", " +
            nameof(Users.Password) + " = @" + nameof(Users.Password) + ", " +
            nameof(Users.CreationDate) + " = @" + nameof(Users.CreationDate) + ", " +
            nameof(Users.IsActivate) + " = @" + nameof(Users.IsActivate) + ", " +
            nameof(Users.PasswordChanged) + " = @" + nameof(Users.PasswordChanged) + " Where " +
            nameof(Users.UserId) + " = @" + nameof(Users.UserId);

        public const string SelectUserIdFromUser =
            "SELECT " +
            nameof(Users.UserId) + "," +
            nameof(Users.Mobile) + " from " +
            nameof(Users);

        public const string SelectAllFromUser =
            "SELECT * from " +
            nameof(Users) + " Where " +
            "{0} = '{1}' AND " +
            nameof(Users.Password) + " = '{2}'";

        public const string SelectAllFromUserWithEmailAndMobile =
            "SELECT * from " +
            nameof(Users) + " Where " +
            nameof(Users.EmailId) + " = '{0}' AND " +
            nameof(Users.Mobile) + " = '{1}'";
    }
}
