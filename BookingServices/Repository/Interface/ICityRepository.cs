using CommonServices.Model.City;
using System.Collections.Generic;

namespace BookingServices.Repository.Interface
{
    public interface ICityRepository
    {
        bool AddOrUpdateCity(List<City> city);
    }
}
