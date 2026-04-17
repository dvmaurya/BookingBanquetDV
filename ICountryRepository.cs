using CommonServices.Model.DbModel;
using System.Collections.Generic;

namespace BookingServices.Repository.Interface
{
    public interface ICountryRepository
    {
        bool AddOrUpdateCountry(Country country);
        IList<Country> GetCountryData();
    }
}
