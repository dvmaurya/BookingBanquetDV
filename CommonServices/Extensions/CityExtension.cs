using CommonServices.Model.City;
using DbModel = CommonServices.Model.DbModel;

namespace CommonServices.Extensions
{
    public static class CityExtension
    {
        public static DbModel.City ToDbCity(this City city)
        {
            DbModel.City dbState = new DbModel.City
            {
                Id = city.Id,
                Latitude = city.Latitude,
                Longitude = city.Longitude,
                Name = city.Name,
                StateId = city.StateId
            };

            return dbState;
        }

        public static City ToCity(this DbModel.City city)
        {
            City dbState = new City
            {
                Id = city.Id,
                Latitude = city.Latitude,
                Longitude = city.Longitude,
                Name = city.Name,
                StateId = city.StateId
            };

            return dbState;
        }
    }
}
