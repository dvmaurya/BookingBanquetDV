using CommonServices.Model.DbModel;

namespace DataAccessLibrary.DML
{
    public static class BookingQuery
    {
        public const string CreateBookingTable =
            "CREATE TABLE IF NOT " +
            "EXISTS " + nameof(BookingDetails) +
            " ( " + nameof(BookingDetails.Id) + " INTEGER PRIMARY KEY, " +
            " " + nameof(BookingDetails.CustomerName) + " NVARCHAR(2048) NUll," +
            " " + nameof(BookingDetails.MobileNo) + " NVARCHAR(2048) NUll," +
            " " + nameof(BookingDetails.EmailId) + " NVARCHAR(2048) NUll," +
            " " + nameof(BookingDetails.BookingDate) + " NVARCHAR(2048) NUll," +
            " " + nameof(BookingDetails.BookedBy) + " NVARCHAR(2048) NUll," +
            " " + nameof(BookingDetails.TotalRooms) + " NVARCHAR(2048) NUll," +
            " " + nameof(BookingDetails.TotalFlours) + " NVARCHAR(2048) NUll," +
            " " + nameof(BookingDetails.TotalAmount) + " NVARCHAR(2048) NUll," +
            " " + nameof(BookingDetails.Advanced) + " NVARCHAR(2048) NUll," +
            " " + nameof(BookingDetails.Balance) + " NVARCHAR(2048) NUll," +
            " " + nameof(BookingDetails.Confirmation) + " NVARCHAR(2048) NUll," +
            " " + nameof(BookingDetails.CreationDate) + " NVARCHAR(2048) NUll," +
            " " + nameof(BookingDetails.UserId) + " NVARCHAR(2048) NUll, " +
            " " + nameof(BookingDetails.IsEmailSent) + " NVARCHAR(2048) NUll, " +
            " " + nameof(BookingDetails.Address) + " NVARCHAR(2048) NUll, " +
            " " + nameof(BookingDetails.Country) + " NVARCHAR(2048) NUll, " +
            " " + nameof(BookingDetails.State) + " NVARCHAR(2048) NUll, " +
            " " + nameof(BookingDetails.City) + " NVARCHAR(2048) NUll)";

        public const string UpdateBookingDetail =
            "UPDATE " +
            nameof(BookingDetails) + " SET " +
            nameof(BookingDetails.CustomerName) + " = @" + nameof(BookingDetails.CustomerName) + ", " +
            nameof(BookingDetails.MobileNo) + " = @" + nameof(BookingDetails.MobileNo) + ", " +
            nameof(BookingDetails.EmailId) + " = @" + nameof(BookingDetails.EmailId) + ", " +
            nameof(BookingDetails.BookingDate) + " = @" + nameof(BookingDetails.BookingDate) + ", " +
            nameof(BookingDetails.BookedBy) + " = @" + nameof(BookingDetails.BookedBy) + ", " +
            nameof(BookingDetails.TotalRooms) + " = @" + nameof(BookingDetails.TotalRooms) + ", " +
            nameof(BookingDetails.TotalFlours) + " = @" + nameof(BookingDetails.TotalFlours) + ", " +
            nameof(BookingDetails.TotalAmount) + " = @" + nameof(BookingDetails.TotalAmount) + ", " +
            nameof(BookingDetails.Advanced) + " = @" + nameof(BookingDetails.Advanced) + ", " +
            nameof(BookingDetails.Balance) + " = @" + nameof(BookingDetails.Balance) + ", " +
            nameof(BookingDetails.Confirmation) + " = @" + nameof(BookingDetails.Confirmation) + ", " +
            nameof(BookingDetails.IsEmailSent) + " = @" + nameof(BookingDetails.IsEmailSent) + ", " +
            nameof(BookingDetails.Address) + " = @" + nameof(BookingDetails.Address) + ", " +
            nameof(BookingDetails.Country) + " = @" + nameof(BookingDetails.Country) + ", " +
            nameof(BookingDetails.State) + " = @" + nameof(BookingDetails.State) + ", " +
            nameof(BookingDetails.City) + " = @" + nameof(BookingDetails.City) + " Where " +
            nameof(BookingDetails.Id) + " = @" + nameof(BookingDetails.Id);

        public const string InsertInBooking =
            "INSERT INTO " + nameof(BookingDetails) + " VALUES (" +
            "NULL, " +
            "@" + nameof(BookingDetails.CustomerName) + "," +
            "@" + nameof(BookingDetails.MobileNo) + "," +
            "@" + nameof(BookingDetails.EmailId) + "," +
            "@" + nameof(BookingDetails.BookingDate) + "," +
            "@" + nameof(BookingDetails.BookedBy) + "," +
            "@" + nameof(BookingDetails.TotalRooms) + "," +
            "@" + nameof(BookingDetails.TotalFlours) + "," +
            "@" + nameof(BookingDetails.TotalAmount) + "," +
            "@" + nameof(BookingDetails.Advanced) + "," +
            "@" + nameof(BookingDetails.Balance) + "," +
            "@" + nameof(BookingDetails.Confirmation) + "," +
            "@" + nameof(BookingDetails.CreationDate) + "," +
            "@" + nameof(BookingDetails.UserId) + "," +
            "@" + nameof(BookingDetails.IsEmailSent) + "," +
            "@" + nameof(BookingDetails.Address) + "," +
            "@" + nameof(BookingDetails.Country) + "," +
            "@" + nameof(BookingDetails.State) + "," +
            "@" + nameof(BookingDetails.City) + ");";

        public const string SelectAllFromBooking =
            "SELECT * from " + nameof(BookingDetails);

        public const string DeleteBooking =
            "DELETE FROM " + nameof(BookingDetails) +
            " WHERE " + nameof(BookingDetails.Id) + " = {0}";
    }
}
