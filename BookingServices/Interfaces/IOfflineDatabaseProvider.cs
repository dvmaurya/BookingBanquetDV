using CommonServices.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingServices.Interfaces
{
    public interface IOfflineDatabaseProvider
    {
        Task CreateOfflineDatabaseAsync();
        bool AddOrUpdateUsers(Users users);
        List<string> GetAllUsers();
        void AddOrUpdateLoginDetail(LoginDetail loginDetail);
        IDictionary<string, string> GetDataForLogin(string userName, string password);
        IDictionary<string, string> GetPreviousLoginDetail();
        IDictionary<string, string> GetDataForOTP(string emailId, string mobileNo);
    }
}
