using CommonServices.Model.City;
using System.Collections.Generic;

namespace BookingServices.Interfaces
{
    public interface ICountryDataProvider
    {
        IList<Country> GetCountryData();
    }
}
