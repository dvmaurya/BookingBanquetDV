using BookingServices.Interfaces;
using BookingServices.Repository.Interface;
using CommonServices.Extensions;
using CommonServices.Model.City;
using System;
using System.Collections.Generic;
using DBModel = CommonServices.Model.DbModel;

namespace BookingServices.DataProvider
{
    public class CountryDataProvider : ICountryDataProvider
    {
        private readonly ICountryRepository _countryRepository;

        public CountryDataProvider(
            ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository ?? throw new ArgumentNullException(nameof(countryRepository));
        }

        public IList<Country> GetCountryData()
        {
            IList<DBModel.Country> dbCountryList = _countryRepository.GetCountryData();

            IList<Country> countryList = new List<Country>();

            foreach (DBModel.Country dbCountry in dbCountryList)
                countryList.Add(dbCountry.ToCountry());

            return countryList;
        }
    }
}
