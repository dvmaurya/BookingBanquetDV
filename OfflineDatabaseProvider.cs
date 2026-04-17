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
        private readonly IUserCredRepository _userRepository;
        private readonly ILoginRepository _loginRepository;
        private readonly ICountryRepository _countryRepository;

        public OfflineDatabaseProvider(
            IUserCredRepository userRepository,
            ILoginRepository loginRepository,
            ICountryRepository countryRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _loginRepository = loginRepository ?? throw new ArgumentNullException(nameof(loginRepository));
            _countryRepository = countryRepository ?? throw new ArgumentNullException(nameof(countryRepository));
        }

        public void CreateOfflineDatabase()
        {
            try
            {
                IList<string> tables = GetTables();

                foreach (string table in tables)
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

        public bool AddOrUpdateUsers(UsersCred usersCred)
        {
            DBModel.UsersCred dbUserCred = usersCred.ToDbUsers();
            return _userRepository.AddOrUpdateUsersCred(dbUserCred);
        }

        public List<String> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        public IDictionary<string, string> GetUserDetails(string emailId, string mobileNo)
        {
            return _userRepository.GetUserDetails(emailId, mobileNo);
        }

        public IDictionary<string, string> GetDataForLogin(string userName, string password)
        {
            return _loginRepository.GetDataForLogin(userName, password);
        }

        private IList<string> GetTables()
        {
            List<string> tableTypes = new List<string>
            {
                nameof(DBModel.UsersCred),
                nameof(DBModel.UsersPersonalDetail),
                nameof(DBModel.LoginDetail),
                nameof(DBModel.BookingDetails),
                nameof(DBModel.Country),
                nameof(DBModel.PhotoDetails),
                nameof(DBModel.CustomerPhotoDetail),
                nameof(DBModel.BanquetDetail),
            };

            return tableTypes;
        }

        public void AddOrUpdateLoginDetail(LoginDetail loginDetail)
        {
            _loginRepository.AddOrUpdateLoginDetail(loginDetail.ToDbLoginDetail());
        }

        public IDictionary<string, string> GetPreviousLoginDetail()
        {
            return _loginRepository.GetPreviousLoginDetail();
        }

        public UsersCred GetUsersDetail(string userId)
        {
            DBModel.UsersCred userCredDetail = _userRepository.GetUsersDetail(userId);

            return userCredDetail.ToUsersCred();
        }
    }
}
