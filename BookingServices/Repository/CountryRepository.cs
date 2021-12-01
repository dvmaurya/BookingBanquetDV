using BookingServices.Repository.Interface;
using CommonServices.Extensions;
using CommonServices.Logger;
using CommonServices.Model.DbModel;
using DataAccessLibrary;
using DataAccessLibrary.DML;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace BookingServices.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private string _dbpath;
        private static string _fileName = "Filename={0}";

        public bool AddOrUpdateCountry(Country country)
        {
            try
            {
                _dbpath = DataAccess.GetDbPath();

                using (SqliteConnection db = new SqliteConnection(_fileName.FormatString(_dbpath)))
                {
                    db.Open();

                    SqliteCommand insertCommand = new SqliteCommand
                    {
                        Connection = db,
                        CommandText = CountryQuery.InsertInCountry
                    };

                    insertCommand.Parameters.AddWithValue("@" + nameof(country.Name), country.Name);
                    insertCommand.Parameters.AddWithValue("@" + nameof(country.Capital), country.Capital);
                    insertCommand.Parameters.AddWithValue("@" + nameof(country.Currency), country.Currency);
                    insertCommand.Parameters.AddWithValue("@" + nameof(country.CurrencySymbol), country.CurrencySymbol);
                    insertCommand.Parameters.AddWithValue("@" + nameof(country.Emoji), country.Emoji);
                    insertCommand.Parameters.AddWithValue("@" + nameof(country.EmojiU), country.EmojiU);
                    insertCommand.Parameters.AddWithValue("@" + nameof(country.Iso2), country.Iso2);
                    insertCommand.Parameters.AddWithValue("@" + nameof(country.Iso3), country.Iso3);
                    insertCommand.Parameters.AddWithValue("@" + nameof(country.Latitude), country.Latitude);
                    insertCommand.Parameters.AddWithValue("@" + nameof(country.Longitude), country.Longitude);
                    insertCommand.Parameters.AddWithValue("@" + nameof(country.Native), country.Native);
                    insertCommand.Parameters.AddWithValue("@" + nameof(country.NumericCode), country.NumericCode);
                    insertCommand.Parameters.AddWithValue("@" + nameof(country.PhoneCode), country.PhoneCode);
                    insertCommand.Parameters.AddWithValue("@" + nameof(country.Region), country.Region);
                    insertCommand.Parameters.AddWithValue("@" + nameof(country.Subregion), country.Subregion);
                    insertCommand.Parameters.AddWithValue("@" + nameof(country.Tld), country.Tld);
                    insertCommand.Parameters.AddWithValue("@" + nameof(country.StateJson), country.StateJson);

                    insertCommand.ExecuteReader();
                    db.Close();

                    return true;
                }
            }
            catch (Exception ex)
            {
                FileLogger.Logger.Info("Country Exception: ".ConcatString(ex.Message));
                return false;
            }
        }

        public IList<Country> GetCountryData()
        {
            IList<Country> countries = new List<Country>();
            Country country = null;
            _dbpath = DataAccess.GetDbPath();

            using (SqliteConnection db = new SqliteConnection(_fileName.FormatString(_dbpath)))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand
                    (CountryQuery.GetAllCountryData, db);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    country = new Country
                    {
                        Id = query.GetString(0).ToInt64(),
                        Name = query.GetString(1),
                        Capital = query.GetString(2),
                        Currency = query.GetString(3),
                        CurrencySymbol = query.GetString(4),
                        Emoji = query.GetString(5),
                        EmojiU = query.GetString(6),
                        Iso2 = query.GetString(7),
                        Iso3 = query.GetString(8),
                        Latitude = query.GetString(9),
                        Longitude = query.GetString(10),
                        Native = query.GetString(11),
                        NumericCode = query.GetString(12),
                        PhoneCode = query.GetString(13),
                        Region = query.GetString(14),
                        Subregion = query.GetString(15),
                        Tld = query.GetString(16),
                        StateJson = query.GetString(17)
                    };

                    countries.Add(country);
                }

                db.Close();
            }

            return countries;
        }
    }
}
