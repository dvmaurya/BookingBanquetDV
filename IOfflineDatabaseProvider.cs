using CommonServices.Model;
using CommonServices.Model.City;
using System.Collections.Generic;
using DbModel = CommonServices.Model.DbModel;

namespace BookingServices.Interfaces
{
    public interface IOfflineDatabaseProvider
    {
        void CreateOfflineDatabase();
        bool AddOrUpdateUsers(UsersCred usersCred);
        List<string> GetAllUsers();
        void AddOrUpdateLoginDetail(LoginDetail loginDetail);
        IDictionary<string, string> GetDataForLogin(string userName, string password);
        IDictionary<string, string> GetPreviousLoginDetail();
        IDictionary<string, string> GetUserDetails(string emailId, string mobileNo);
        void StoreCountryData(List<Country> country);
        UsersCred GetUsersDetail(string userId);
    }
}
