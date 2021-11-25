using CommonServices.Model;
using System.Collections.Generic;

namespace BookingServices.Repository.Interface
{
    public interface IUserRepository
    {
        bool AddOrUpdateUsers(Users users);
        List<string> GetAllUsers();
        IDictionary<string, string> GetDataForOTP(string emailId, string mobileNo);
    }
}
