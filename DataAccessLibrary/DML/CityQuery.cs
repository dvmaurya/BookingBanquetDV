using CommonServices.Model.DbModel;

namespace DataAccessLibrary.DML
{
    public static class CityQuery
    {
        public const string CreateCityTable =
            "CREATE TABLE IF NOT " +
            "EXISTS " + nameof(City) +
            " ( " + nameof(City.Id) + " INTEGER PRIMARY KEY, " +
            " " + nameof(City.Name) + " NVARCHAR(2048) NUll," +
            " " + nameof(City.StateId) + " NVARCHAR(2048) NUll," +
            " " + nameof(City.Latitude) + " NVARCHAR(2048) NUll," +
            " " + nameof(City.Longitude) + " NVARCHAR(2048) NUll)";

        public const string InsertInCity =
            "INSERT INTO " + nameof(City) + " VALUES (" +
            "NULL, " +
             "@" + nameof(City.Name) + "," +
            "@" + nameof(City.StateId) + "," +
            "@" + nameof(City.Latitude) + "," +
            "@" + nameof(City.Longitude) + ");";

        public const string UpdateInCity =
            "UPDATE " +
            nameof(City) + " SET " +
            nameof(City.Name) + " = @" + nameof(City.Name) + ", " +
            nameof(City.Latitude) + " = @" + nameof(City.StateId) + ", " +
            nameof(City.Longitude) + " = @" + nameof(City.Latitude) + ", " +
            nameof(City.StateId) + " = @" + nameof(City.Longitude) + " Where " +
            nameof(City.Id) + " = @" + nameof(City.Id);
    }
}
