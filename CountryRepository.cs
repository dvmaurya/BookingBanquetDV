using BookingServices.Repository.Interface;
using CommonServices.Analytcis;
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
    public class CountryRepository : BaseRepository, ICountryRepository
    {
        private string _dbpath;
        private static string _fileName = "Filename={0}";

        public CountryRepository(
            IAnalyticsManager analyticsManager)
            : base(analyticsManager)
        {

        }
        public bool AddOrUpdateCountry(Country country)
        {
            try
            {
                if(country != null)
                { 
                _dbpath = DataAccess.GetDbPath();

                    using (SqliteConnection db = new SqliteConnection(_fileName.FormatString(_dbpath)))
                    {
                        db.Open();

                        using (SqliteCommand insertCommand = new SqliteCommand(CountryQuery.InsertInCountry, db))
                        {
                            insertCommand.Parameters.AddWithValue("@" + nameof(country.Name), country.Name ?? string.Empty);
                            insertCommand.Parameters.AddWithValue("@" + nameof(country.Capital), country.Capital ?? string.Empty);
                            insertCommand.Parameters.AddWithValue("@" + nameof(country.Currency), country.Currency ?? string.Empty);
                            insertCommand.Parameters.AddWithValue("@" + nameof(country.CurrencySymbol), country.CurrencySymbol ?? string.Empty);
                            insertCommand.Parameters.AddWithValue("@" + nameof(country.Emoji), country.Emoji ?? string.Empty);
                            insertCommand.Parameters.AddWithValue("@" + nameof(country.EmojiU), country.EmojiU ?? string.Empty);
                            insertCommand.Parameters.AddWithValue("@" + nameof(country.Iso2), country.Iso2 ?? string.Empty);
                            insertCommand.Parameters.AddWithValue("@" + nameof(country.Iso3), country.Iso3 ?? string.Empty);
                            insertCommand.Parameters.AddWithValue("@" + nameof(country.Latitude), country.Latitude ?? string.Empty);
                            insertCommand.Parameters.AddWithValue("@" + nameof(country.Longitude), country.Longitude ?? string.Empty);
                            insertCommand.Parameters.AddWithValue("@" + nameof(country.Native), country.Native ?? string.Empty);
                            insertCommand.Parameters.AddWithValue("@" + nameof(country.NumericCode), country.NumericCode ?? string.Empty);
                            insertCommand.Parameters.AddWithValue("@" + nameof(country.PhoneCode), country.PhoneCode ?? string.Empty);
                            insertCommand.Parameters.AddWithValue("@" + nameof(country.Region), country.Region ?? string.Empty);
                            insertCommand.Parameters.AddWithValue("@" + nameof(country.Subregion), country.Subregion ?? string.Empty);
                            insertCommand.Parameters.AddWithValue("@" + nameof(country.Tld), country.Tld ?? string.Empty);
                            insertCommand.Parameters.AddWithValue("@" + nameof(country.StateJson), country.StateJson ?? string.Empty);

                            insertCommand.ExecuteReader();
                        }

                        db.Close();

                        TraceEvents(
                            analyticsEvent: AnalyticConstants.Application,
                            action: $"{AnalyticConstants.Add} " +
                                    $"{AnalyticConstants.Country} ",
                            label: $"{AnalyticConstants.Add} " +
                                    $"{AnalyticConstants.Country} " +
                                    $"in {AnalyticConstants.Database} " +
                                    $"{AnalyticConstants.Successfully}");

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogger.Logger.Info("Country Exception: ".ConcatString(ex.Message));
                TraceExceptions("Country exception: ".ConcatString(ex.Message));
            }

            return false;
        }

        public IList<Country> GetCountryData()
        {
            IList<Country> countries = new List<Country>();
            Country country = null;
            _dbpath = DataAccess.GetDbPath();

            try
            {
                using (SqliteConnection db = new SqliteConnection(_fileName.FormatString(_dbpath)))
                {
                    db.Open();
                    using (SqliteCommand selectCommand = new SqliteCommand(CountryQuery.GetAllCountryData, db))
                    {
                        using (SqliteDataReader query = selectCommand.ExecuteReader())
                        {
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
                        }
                    }

                    db.Close();
                }
            }
            catch(Exception ex)
            {
                FileLogger.Logger.Info("Get country from database Exception: ".ConcatString(ex.Message));
                TraceExceptions("Get country from database exception: ".ConcatString(ex.Message));
            }

            return countries;
        }
    }
}
