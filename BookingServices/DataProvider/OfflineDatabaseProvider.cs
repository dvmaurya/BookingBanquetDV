using BookingServices.Interfaces;
using BookingServices.Repository.Interface;
using CommonServices.ConstantData;
using CommonServices.Extensions;
using CommonServices.Logger;
using CommonServices.Model;
using DataAccessLibrary;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingServices.DataProvider
{
    public class OfflineDatabaseProvider : IOfflineDatabaseProvider
    {
        private readonly IDbContext _dbContext;
        private readonly IUserRepository _userRepository;
        private readonly ILoginRepository _loginRepository;

        public OfflineDatabaseProvider(
            IDbContext dbContext,
            IUserRepository userRepository,
            ILoginRepository loginRepository)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _loginRepository = loginRepository ?? throw new ArgumentNullException(nameof(loginRepository));
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
                nameof(BookingDetails)
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
    }
}
