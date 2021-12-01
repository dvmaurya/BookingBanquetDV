using CommonServices.Model.City;
using DbModel = CommonServices.Model.DbModel;

namespace CommonServices.Extensions
{
    public static class StateExtension
    {
        public static DbModel.State ToDbState(this State state)
        {
            DbModel.State dbState = new DbModel.State
            {
                Id = state.Id,
                Latitude = state.Latitude,
                Longitude = state.Longitude,
                Name = state.Name,
                CountryId = state.CountryId,
                StateCode = state.StateCode,
                Type = state.Type.ConvertToString()
            };

            return dbState;
        }

        public static State ToState(this DbModel.State state)
        {
            State dbState = new State
            {
                Id = state.Id,
                Latitude = state.Latitude,
                Longitude = state.Longitude,
                Name = state.Name,
                CountryId = state.CountryId,
                StateCode = state.StateCode,
                Type = state.Type.ConvertToString()
            };

            return dbState;
        }
    }
}
