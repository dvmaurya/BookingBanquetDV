using BookingServices.Interfaces;
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

        public OfflineDatabaseProvider(IDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException("dbContext");
        }

        public async Task CreateOfflineDatabaseAsync()
        {
            try
            {
                using (_dbContext)
                {
                    await _dbContext.CreateTables(GetTables()).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                FileLogger.Logger.Info("Offline database creation failed: ".ConcatString(ex.Message));
            }
        }

        public bool AddOrUpdateUsers(Users users)
        {
            return DataAccess.AddOrUpdateUsers(users);
        }

        public List<String> GetAllUsers(string TableName)
        {
            return DataAccess.GetAllUsers(TableName);
        }

        public IDictionary<string, string> GetDataForLogin(string userName, string password)
        {
            return DataAccess.GetDataForLogin(userName, password);
        }

        private IList<Type> GetTables()
        {
            List<Type> tableTypes = new List<Type>
            {
                typeof(Users)
            };

            return tableTypes;
        }

        public void AddOrUpdateLoginDetail(LoginDetail loginDetail)
        {
            DataAccess.AddOrUpdateLoginDetail(loginDetail);
        }

        public IDictionary<string, string> GetPreviousLoginDetail()
        {
            return DataAccess.GetPreviousLoginDetail();
        }
    }
}
