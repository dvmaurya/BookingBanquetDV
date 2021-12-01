using CommonServices.Model.DbModel;

namespace DataAccessLibrary.DML
{
    public class StateQuery
    {
        public const string CreateStateTable =
            "CREATE TABLE IF NOT " +
            "EXISTS " + nameof(State) +
            " ( " + nameof(State.Id) + " INTEGER PRIMARY KEY, " +
            " " + nameof(State.Name) + " NVARCHAR(2048) NUll," +
            " " + nameof(State.CountryId ) + " NVARCHAR(2048) NUll," +
            " " + nameof(State.StateCode) + " NVARCHAR(2048) NUll," +
            " " + nameof(State.Latitude) + " NVARCHAR(2048) NUll," +
            " " + nameof(State.Longitude) + " NVARCHAR(2048) NUll)";

        public const string InsertInState =
            "INSERT INTO " + nameof(State) + " VALUES (" +
            "NULL, " +
            "@" + nameof(State.Name) + "," +
            "@" + nameof(State.CountryId) + "," +
            "@" + nameof(State.StateCode) + "," +
            "@" + nameof(State.Latitude) + "," +
            "@" + nameof(State.Longitude) + ");";
    }
}
