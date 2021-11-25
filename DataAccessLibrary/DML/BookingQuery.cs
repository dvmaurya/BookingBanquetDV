using CommonServices.Model;

namespace DataAccessLibrary.DML
{
    public static class BookingQuery
    {
        public const string CreateBookingTable =
            "CREATE TABLE IF NOT " +
            "EXISTS " + nameof(BookingDetails) +
            " ( " + nameof(BookingDetails.Id) + " INTEGER PRIMARY KEY, " +
            " " + nameof(BookingDetails.CustomerName) + " NVARCHAR(2048) NUll," +
            " " + nameof(BookingDetails.BookingDate) + " NVARCHAR(2048) NUll," +
            " " + nameof(BookingDetails.BookedBy) + " NVARCHAR(2048) NUll," +
            " " + nameof(BookingDetails.NumberOfRooms) + " NVARCHAR(2048) NUll," +
            " " + nameof(BookingDetails.NumberOfFlours) + " NVARCHAR(2048) NUll," +
            " " + nameof(BookingDetails.TotalAmount) + " NVARCHAR(2048) NUll," +
            " " + nameof(BookingDetails.Advanced) + " NVARCHAR(2048) NUll," +
            " " + nameof(BookingDetails.Balance) + " NVARCHAR(2048) NUll," +
            " " + nameof(BookingDetails.Confirmation) + " NVARCHAR(2048) NUll," +
            " " + nameof(BookingDetails.CreationDate) + " NVARCHAR(2048) NUll," +
            " " + nameof(BookingDetails.PhotoName) + " NVARCHAR(2048) NUll," +
            " " + nameof(BookingDetails.UserId) + " NVARCHAR(2048) NUll)";

        public const string UpdateBookingDetail =
            "UPDATE " +
            nameof(BookingDetails) + " SET " +
            nameof(BookingDetails.CustomerName) + " = @" + nameof(BookingDetails.CustomerName) + ", " +
            nameof(BookingDetails.BookingDate) + " = @" + nameof(BookingDetails.BookingDate) + ", " +
            nameof(BookingDetails.BookedBy) + " = @" + nameof(BookingDetails.BookedBy) + ", " +
            nameof(BookingDetails.NumberOfRooms) + " = @" + nameof(BookingDetails.NumberOfRooms) + ", " +
            nameof(BookingDetails.NumberOfFlours) + " = @" + nameof(BookingDetails.NumberOfFlours) + ", " +
            nameof(BookingDetails.TotalAmount) + " = @" + nameof(BookingDetails.TotalAmount) + ", " +
            nameof(BookingDetails.Advanced) + " = @" + nameof(BookingDetails.Advanced) + ", " +
            nameof(BookingDetails.Balance) + " = @" + nameof(BookingDetails.Balance) + ", " +
            nameof(BookingDetails.Confirmation) + " = @" + nameof(BookingDetails.Confirmation) + ", " +
            nameof(BookingDetails.PhotoName) + " = @" + nameof(BookingDetails.PhotoName) + " Where " +
            nameof(BookingDetails.Id) + " = @" + nameof(BookingDetails.Id);

        public const string InsertInBooking =
            "INSERT INTO " + nameof(BookingDetails) + " VALUES (" +
            "NULL, " +
            "@" + nameof(BookingDetails.CustomerName) + "," +
            "@" + nameof(BookingDetails.BookingDate) + "," +
            "@" + nameof(BookingDetails.BookedBy) + "," +
            "@" + nameof(BookingDetails.NumberOfRooms) + "," +
            "@" + nameof(BookingDetails.NumberOfFlours) + "," +
            "@" + nameof(BookingDetails.TotalAmount) + "," +
            "@" + nameof(BookingDetails.Advanced) + "," +
            "@" + nameof(BookingDetails.Balance) + "," +
            "@" + nameof(BookingDetails.Confirmation) + "," +
            "@" + nameof(BookingDetails.CreationDate) + "," +
            "@" + nameof(BookingDetails.PhotoName) + "," +
            "@" + nameof(BookingDetails.UserId) + ");";

        public const string SelectAllFromBooking =
            "SELECT * from " +
            nameof(BookingDetails) + " Where " +
            nameof(BookingDetails.UserId) + " = '{0}'";

        public const string DeleteBooking =
            "DELETE FROM " + nameof(BookingDetails) +
            " WHERE " + nameof(BookingDetails.Id) + " = {0}";
    }
}
