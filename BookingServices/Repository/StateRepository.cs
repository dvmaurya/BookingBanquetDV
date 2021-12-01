using BookingServices.Repository.Interface;
using CommonServices.Extensions;
using CommonServices.Logger;
using CommonServices.Model.City;
using DataAccessLibrary;
using DataAccessLibrary.DML;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace BookingServices.Repository
{
    public class StateRepository : IStateRepository
    {

        private string _dbpath;
        private static string _fileName = "Filename={0}";

        public bool AddOrUpdateState(List<State> stateList)
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
                        CommandText = StateQuery.InsertInState
                    };

                    foreach (State state in stateList)
                    {
                        insertCommand.Parameters.AddWithValue("@" + nameof(state.Name), state.Name);
                        insertCommand.Parameters.AddWithValue("@" + nameof(state.CountryId), state.CountryId);
                        insertCommand.Parameters.AddWithValue("@" + nameof(state.StateCode), state.StateCode);
                        insertCommand.Parameters.AddWithValue("@" + nameof(state.Latitude), state.Latitude);
                        insertCommand.Parameters.AddWithValue("@" + nameof(state.Longitude), state.Longitude);

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
