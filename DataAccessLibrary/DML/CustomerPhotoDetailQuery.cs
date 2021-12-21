using CommonServices.Model.DbModel;

namespace DataAccessLibrary.DML
{
    public class CustomerPhotoDetailQuery
    {
        public const string CreatePhotoDetailTable =
            "CREATE TABLE IF NOT " +
            "EXISTS " + nameof(CustomerPhotoDetail) +
            " ( " + nameof(CustomerPhotoDetail.Id) + " INTEGER PRIMARY KEY, " +
            " " + nameof(CustomerPhotoDetail.PhotoName) + " NVARCHAR(2048) NUll," +
            " " + nameof(CustomerPhotoDetail.Date) + " NVARCHAR(2048) NUll," +
            " " + nameof(CustomerPhotoDetail.FileType) + " NVARCHAR(2048) NUll," +
            " " + nameof(CustomerPhotoDetail.Size) + " NVARCHAR(2048) NUll," +
            " " + nameof(CustomerPhotoDetail.EmailId) + " NVARCHAR(2048) NUll," +
            " " + nameof(CustomerPhotoDetail.BookingId) + " NVARCHAR(2048) NUll)";

        public const string UpdatePhotoDetail =
            "UPDATE " +
            nameof(CustomerPhotoDetail) + " SET " +
            nameof(CustomerPhotoDetail.PhotoName) + " = @" + nameof(CustomerPhotoDetail.PhotoName) + ", " +
            nameof(CustomerPhotoDetail.Date) + " = @" + nameof(CustomerPhotoDetail.Date) + ", " +
            nameof(CustomerPhotoDetail.FileType) + " = @" + nameof(CustomerPhotoDetail.FileType) + ", " +
            nameof(CustomerPhotoDetail.Size) + " = @" + nameof(CustomerPhotoDetail.Size) + ", " +
            nameof(CustomerPhotoDetail.EmailId) + " = @" + nameof(CustomerPhotoDetail.EmailId) + ", " +
            nameof(CustomerPhotoDetail.BookingId) + " = @" + nameof(CustomerPhotoDetail.BookingId) + " Where " +
            nameof(CustomerPhotoDetail.Id) + " = @" + nameof(PhotoDetails.Id);

        public const string InsertInPhotoDetail =
           "INSERT INTO " + nameof(CustomerPhotoDetail) + " VALUES (" +
           "NULL, " +
           "@" + nameof(CustomerPhotoDetail.PhotoName) + "," +
           "@" + nameof(CustomerPhotoDetail.Date) + "," +
           "@" + nameof(CustomerPhotoDetail.FileType) + "," +
           "@" + nameof(CustomerPhotoDetail.Size) + "," +
           "@" + nameof(CustomerPhotoDetail.EmailId) + "," +
           "@" + nameof(CustomerPhotoDetail.BookingId) + ");";

        public const string GetActivePhoto =
            "SELECT * from " +
            nameof(CustomerPhotoDetail) + " Where " +
            nameof(CustomerPhotoDetail.BookingId) + " = {0}";

        public const string DeleteCustomerPhoto =
            "DELETE FROM " + nameof(CustomerPhotoDetail) +
            " WHERE " + nameof(CustomerPhotoDetail.BookingId) + " = {0}";
    }
}
