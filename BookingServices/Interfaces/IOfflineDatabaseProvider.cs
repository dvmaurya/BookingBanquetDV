using CommonServices.Model;
using CommonServices.Model.City;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingServices.Interfaces
{
    public interface IOfflineDatabaseProvider
    {
        void CreateOfflineDatabase();
        bool AddOrUpdateUsers(Users users);
        List<string> GetAllUsers();
        void AddOrUpdateLoginDetail(LoginDetail loginDetail);
        IDictionary<string, string> GetDataForLogin(string userName, string password);
        IDictionary<string, string> GetPreviousLoginDetail();
        IDictionary<string, string> GetDataForOTP(string emailId, string mobileNo);
        void StoreCountryData(List<Country> country);
        Users GetUsersDetail(string userId);
    }
}
