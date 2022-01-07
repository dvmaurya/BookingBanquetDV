using CommonServices.Model.DbModel;

namespace DataAccessLibrary.DML
{
    public static class SignupQuery
    {
        public const string CreateSignupTable =
            "CREATE TABLE IF NOT " +
            "EXISTS " + nameof(UsersCred) +
            " ( " + nameof(UsersCred.Id) + " INTEGER PRIMARY KEY, " +
            " " + nameof(UsersCred.UserId) + " NVARCHAR(2048) NUll," +
            " " + nameof(UsersCred.EmailId) + " NVARCHAR(2048) NUll," +
            " " + nameof(UsersCred.Mobile) + " NVARCHAR(2048) NUll," +
            " " + nameof(UsersCred.MobilePassword) + " NVARCHAR(2048) NUll," +
            " " + nameof(UsersCred.EmailPassword) + " NVARCHAR(2048) NUll," +
            " " + nameof(UsersCred.CreationDate) + " NVARCHAR(2048) NULL, " +
            " " + nameof(UsersCred.IsAdmin) + " NVARCHAR(2048) NULL, " +
            " " + nameof(UsersCred.PasswordChanged) + " NVARCHAR(2048) NULL)";
        
        public const string InsertInSignup = 
            "INSERT INTO " + nameof(UsersCred) + " VALUES (" +
            "NULL, " +
            "@" + nameof(UsersCred.UserId) + "," +
            "@" + nameof(UsersCred.EmailId) + "," +
            "@" + nameof(UsersCred.Mobile) + "," +
            "@" + nameof(UsersCred.MobilePassword) + "," +
            "@" + nameof(UsersCred.EmailPassword) + "," +
            "@" + nameof(UsersCred.CreationDate) + "," +
            "0, 0);";

        public const string UpdateInSignup = 
            "UPDATE " +
            nameof(UsersCred) + " SET " +
            nameof(UsersCred.UserId) + " = @" + nameof(UsersCred.UserId) + ", " +
            nameof(UsersCred.EmailId) + " = @" + nameof(UsersCred.EmailId) + ", " +
            nameof(UsersCred.Mobile) + " = @" + nameof(UsersCred.Mobile) + ", " +
            nameof(UsersCred.MobilePassword) + " = @" + nameof(UsersCred.MobilePassword) + ", " +
            nameof(UsersCred.EmailPassword) + " = @" + nameof(UsersCred.EmailPassword) + ", " +
            nameof(UsersCred.CreationDate) + " = @" + nameof(UsersCred.CreationDate) + ", " +
            nameof(UsersCred.IsAdmin) + " = @" + nameof(UsersCred.IsAdmin) + ", " +
            nameof(UsersCred.PasswordChanged) + " = @" + nameof(UsersCred.PasswordChanged) + " Where " +
            nameof(UsersCred.UserId) + " = @" + nameof(UsersCred.UserId);

        public const string SelectUserIdFromUser =
            "SELECT " +
            nameof(UsersCred.UserId) + "," +
            nameof(UsersCred.Mobile) + " from " +
            nameof(UsersCred);

        public const string SelectAllFromUserMobile =
            "SELECT * from " +
            nameof(UsersCred) + " Where " +
            "{0} = '{1}' AND " +
            nameof(UsersCred.MobilePassword) + " = '{2}'";

        public const string SelectAllFromUserEmail =
            "SELECT * from " +
            nameof(UsersCred) + " Where " +
            "{0} = '{1}' AND " +
            nameof(UsersCred.EmailPassword) + " = '{2}'";

        public const string SelectAllFromUserWithEmailAndMobile =
            "SELECT * from " +
            nameof(UsersCred) + " Where " +
            nameof(UsersCred.EmailId) + " = '{0}' AND " +
            nameof(UsersCred.Mobile) + " = '{1}'";

        public const string GetUserDetailByUserId =
            "SELECT " +
            nameof(UsersCred.UserId) + "," +
            nameof(UsersCred.FullName) + "," +
            nameof(UsersCred.EmailId) + "," +
            nameof(UsersCred.Mobile) + "," +
            nameof(UsersCred.CreationDate) + " from " +
            nameof(UsersCred) + " Where {0} = '{1}'";
    }
}
