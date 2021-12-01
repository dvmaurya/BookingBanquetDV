using System;
using System.Collections.Generic;
using CommonServices.Extensions;
using CommonServices.Logger;
using CommonServices.Model.City;
using DataAccessLibrary;
using DataAccessLibrary.DML;
using Microsoft.Data.Sqlite;

namespace BookingServices.Repository.Interface
{
    public class CityRepository : ICityRepository
    {
        private string _dbpath;
        private static string _fileName = "Filename={0}";

        public bool AddOrUpdateCity(List<City> cityList)
        {
            try
            {
                string insertOrUpdate = string.Empty;
                _dbpath = DataAccess.GetDbPath();

                using (SqliteConnection db = new SqliteConnection(_fileName.FormatString(_dbpath)))
                {
                    db.Open();

                    SqliteCommand insertCommand = new SqliteCommand
                    {
                        Connection = db,
                        CommandText = CityQuery.InsertInCity
                    };

                    foreach (City city in cityList)
                    {
                        insertCommand.Parameters.AddWithValue("@" + nameof(city.Name), city.Name);
                        insertCommand.Parameters.AddWithValue("@" + nameof(city.StateId), city.StateId);
                        insertCommand.Parameters.AddWithValue("@" + nameof(city.Latitude), city.Latitude);
                        insertCommand.Parameters.AddWithValue("@" + nameof(city.Longitude), city.Longitude);

                        insertCommand.ExecuteReader();
                    }

                    db.Close();

                    return true;
                }
            }
            catch (Exception ex)
            {
                FileLogger.Logger.Info("State Exception: ".ConcatString(ex.Message));
                return false;
            }
        }
    }
}
