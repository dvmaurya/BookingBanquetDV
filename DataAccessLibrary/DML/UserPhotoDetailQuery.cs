using CommonServices.Model.DbModel;

namespace DataAccessLibrary.DML
{
    public static class PhotoDetailQuery
    {
        public const string CreatePhotoDetailTable =
            "CREATE TABLE IF NOT " +
            "EXISTS " + nameof(PhotoDetails) +
            " ( " + nameof(PhotoDetails.Id) + " INTEGER PRIMARY KEY, " +
            " " + nameof(PhotoDetails.PhotoName) + " NVARCHAR(2048) NUll," +
            " " + nameof(PhotoDetails.Date) + " NVARCHAR(2048) NUll," +
            " " + nameof(PhotoDetails.FileType) + " NVARCHAR(2048) NUll," +
            " " + nameof(PhotoDetails.Size) + " NVARCHAR(2048) NUll," +
            " " + nameof(PhotoDetails.EmailId) + " NVARCHAR(2048) NUll," +
            " " + nameof(PhotoDetails.IsPhotoActive) + " NVARCHAR(2048) NUll," +
            " " + nameof(PhotoDetails.IsPhotoRemoved) + " NVARCHAR(2048) NUll)";

        public const string UpdatePhotoDetail =
            "UPDATE " +
            nameof(PhotoDetails) + " SET " +
            nameof(PhotoDetails.PhotoName) + " = @" + nameof(PhotoDetails.PhotoName) + ", " +
            nameof(PhotoDetails.Date) + " = @" + nameof(PhotoDetails.Date) + ", " +
            nameof(PhotoDetails.FileType) + " = @" + nameof(PhotoDetails.FileType) + ", " +
            nameof(PhotoDetails.Size) + " = @" + nameof(PhotoDetails.Size) + ", " +
            nameof(PhotoDetails.EmailId) + " = @" + nameof(PhotoDetails.EmailId) + ", " +
            nameof(PhotoDetails.IsPhotoActive) + " = @" + nameof(PhotoDetails.IsPhotoActive) + ", " +
            nameof(PhotoDetails.IsPhotoRemoved) + " = @" + nameof(PhotoDetails.IsPhotoRemoved) + " Where " +
            nameof(PhotoDetails.Id) + " = @" + nameof(PhotoDetails.Id);

        public const string InsertInPhotoDetail =
           "INSERT INTO " + nameof(PhotoDetails) + " VALUES (" +
           "NULL, " +
           "@" + nameof(PhotoDetails.PhotoName) + "," +
           "@" + nameof(PhotoDetails.Date) + "," +
           "@" + nameof(PhotoDetails.FileType) + "," +
           "@" + nameof(PhotoDetails.Size) + "," +
           "@" + nameof(PhotoDetails.EmailId) + "," +
           "@" + nameof(PhotoDetails.IsPhotoActive) + "," +
           "@" + nameof(PhotoDetails.IsPhotoRemoved) + ");";

        public const string GetPreviousPhoto =
            "SELECT * from " +
            nameof(PhotoDetails) + " Where " +
            nameof(PhotoDetails.EmailId) + " = '{0}' AND " +
            nameof(PhotoDetails.IsPhotoRemoved) + " != '1'";

        public const string GetActivePhoto =
            "SELECT * from " +
            nameof(PhotoDetails) + " Where " +
            nameof(PhotoDetails.EmailId) + " = '{0}' AND " +
            nameof(PhotoDetails.IsPhotoActive) + " = '1'";
    }
}
