using CommonServices.Model.DbModel;

namespace DataAccessLibrary.DML
{
    public class CountryQuery
    {
        public const string CreateCountryTable =
            "CREATE TABLE IF NOT " +
            "EXISTS " + nameof(Country) +
            " ( " + nameof(Country.Id) + " INTEGER PRIMARY KEY, " +
            " " + nameof(Country.Name) + " NVARCHAR(2048) NUll," +
            " " + nameof(Country.Capital) + " NVARCHAR(2048) NUll," +
            " " + nameof(Country.Currency) + " NVARCHAR(2048) NUll," +
            " " + nameof(Country.CurrencySymbol) + " NVARCHAR(2048) NUll," +
            " " + nameof(Country.Emoji) + " NVARCHAR(2048) NUll," +
            " " + nameof(Country.EmojiU) + " NVARCHAR(2048) NUll," +
            " " + nameof(Country.Iso2) + " NVARCHAR(2048) NUll," +
            " " + nameof(Country.Iso3) + " NVARCHAR(2048) NUll," +
            " " + nameof(Country.Latitude) + " NVARCHAR(2048) NUll," +
            " " + nameof(Country.Longitude) + " NVARCHAR(2048) NUll," +
            " " + nameof(Country.Native) + " NVARCHAR(2048) NUll," +
            " " + nameof(Country.NumericCode) + " NVARCHAR(2048) NUll," +
            " " + nameof(Country.PhoneCode) + " NVARCHAR(2048) NUll," +
            " " + nameof(Country.Region) + " NVARCHAR(2048) NUll," +
            " " + nameof(Country.Subregion) + " NVARCHAR(2048) NUll," +
            " " + nameof(Country.Tld) + " NVARCHAR(2048) NUll," +
            " " + nameof(Country.StateJson) + " NVARCHAR(2048) NUll)";

        public const string InsertInCountry =
            "INSERT INTO " + nameof(Country) + " VALUES (" +
            "NULL, " +
            "@" + nameof(Country.Name) + "," +
            "@" + nameof(Country.Capital) + "," +
            "@" + nameof(Country.Currency) + "," +
            "@" + nameof(Country.CurrencySymbol) + "," +
            "@" + nameof(Country.Emoji) + "," +
            "@" + nameof(Country.EmojiU) + "," +
            "@" + nameof(Country.Iso2) + "," +
            "@" + nameof(Country.Iso3) + "," +
            "@" + nameof(Country.Latitude) + "," +
            "@" + nameof(Country.Longitude) + "," +
            "@" + nameof(Country.Native) + "," +
            "@" + nameof(Country.NumericCode) + "," +
            "@" + nameof(Country.PhoneCode) + "," +
            "@" + nameof(Country.Region) + "," +
            "@" + nameof(Country.Subregion) + "," +
            "@" + nameof(Country.Tld) + "," +
            "@" + nameof(Country.StateJson) + ");";

        public const string UpdateInCountry =
            "UPDATE " +
            nameof(Country) + " SET " +
            nameof(Country.Name) + " = @" + nameof(Country.Name) + "," +
            nameof(Country.Capital) + " = @" + nameof(Country.Capital) + "," +
            nameof(Country.Currency) + " = @" + nameof(Country.Currency) + "," +
            nameof(Country.CurrencySymbol) + " = @" + nameof(Country.CurrencySymbol) + "," +
            nameof(Country.Emoji) + " = @" + nameof(Country.Emoji) + "," +
            nameof(Country.EmojiU) + " = @" + nameof(Country.EmojiU) + "," +
            nameof(Country.Iso2) + " = @" + nameof(Country.Iso2) + "," +
            nameof(Country.Iso3) + " = @" + nameof(Country.Iso3) + "," +
            nameof(Country.Latitude) + " = @" + nameof(Country.Latitude) + "," +
            nameof(Country.Longitude) + " = @" + nameof(Country.Longitude) + "," +
            nameof(Country.Native) + " = @" + nameof(Country.Native) + "," +
            nameof(Country.NumericCode) + " = @" + nameof(Country.NumericCode) + "," +
            nameof(Country.PhoneCode) + " = @" + nameof(Country.PhoneCode) + "," +
            nameof(Country.Region) + " = @" + nameof(Country.Region) + "," +
            nameof(Country.Subregion) + " = @" + nameof(Country.Subregion) + "," +
            nameof(Country.StateJson) + " = @" + nameof(Country.StateJson) + " WHERE " +
            nameof(Country.Tld) + " = @" + nameof(Country.Tld) + ");";

        public const string GetAllCountryData =
            "SELECT * from " + nameof(Country);
    }
}
