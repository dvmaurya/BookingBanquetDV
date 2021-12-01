using BookingServices.Interfaces;
using BookingServices.Repository.Interface;
using CommonServices.Extensions;
using CommonServices.Logger;
using CommonServices.Model;
using CommonServices.Model.City;
using DataAccessLibrary;
using System;
using System.Collections.Generic;
using DBModel = CommonServices.Model.DbModel;

namespace BookingServices.DataProvider
{
    public class OfflineDatabaseProvider : IOfflineDatabaseProvider
    {
        private readonly IUserRepository _userRepository;
        private readonly ILoginRepository _loginRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICityRepository _cityRepository;

        public OfflineDatabaseProvider(
            IUserRepository userRepository,
            ILoginRepository loginRepository,
            ICountryRepository countryRepository,
            IStateRepository stateRepository,
            ICityRepository cityRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _loginRepository = loginRepository ?? throw new ArgumentNullException(nameof(loginRepository));
            _countryRepository = countryRepository ?? throw new ArgumentNullException(nameof(countryRepository));
            _stateRepository = stateRepository ?? throw new ArgumentNullException(nameof(stateRepository));
            _cityRepository = cityRepository ?? throw new ArgumentNullException(nameof(cityRepository));
        }

        public void CreateOfflineDatabase()
        {
            try
            {
                IList<string> tables = GetTables();

                foreach (var table in tables)
                    DataAccess.CreateTables(table);
            }
            catch (Exception ex)
            {
                FileLogger.Logger.Info("Offline database creation failed: ".ConcatString(ex.Message));
            }
        }

        public void StoreCountryData(List<Country> countries)
        {
            if (countries != null )
            {
                IList<DBModel.Country> DbCountry = _countryRepository.GetCountryData();

                if (DbCountry != null && DbCountry.Count == 0 || DbCountry == null)
                {
                    foreach (Country country in countries)
                    {
                        DBModel.Country dbCountry = country.ToDbCountry();
                        _countryRepository.AddOrUpdateCountry(dbCountry);
                    }
                }
            }
        }

        public bool AddOrUpdateUsers(Users users)
        {
            return _userRepository.AddOrUpdateUsers(users);
        }

        public List<String> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        public IDictionary<string, string> GetDataForOTP(string emailId, string mobileNo)
        {
            return _userRepository.GetDataForOTP(emailId, mobileNo);
        }

        public IDictionary<string, string> GetDataForLogin(string userName, string password)
        {
            return _loginRepository.GetDataForLogin(userName, password);
        }

        private IList<string> GetTables()
        {
            List<string> tableTypes = new List<string>
            {
                nameof(Users),
                nameof(LoginDetail),
                nameof(BookingDetails),
                nameof(Country),
                nameof(State),
                nameof(City)
            };

            return tableTypes;
        }

        public void AddOrUpdateLoginDetail(LoginDetail loginDetail)
        {
            _loginRepository.AddOrUpdateLoginDetail(loginDetail);
        }

        public IDictionary<string, string> GetPreviousLoginDetail()
        {
            return _loginRepository.GetPreviousLoginDetail();
        }

        public Users GetUsersDetail(string userId)
        {
            return _userRepository.GetUsersDetail(userId);
        }
    }
}
