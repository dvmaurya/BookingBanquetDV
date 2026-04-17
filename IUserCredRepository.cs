using CommonServices.Model.DbModel;
using System.Collections.Generic;

namespace BookingServices.Repository.Interface
{
    public interface IUserCredRepository
    {
        bool AddOrUpdateUsersCred(UsersCred userCred);
        List<string> GetAllUsers();
        IDictionary<string, string> GetUserDetails(string emailId, string mobileNo);
        UsersCred GetUsersDetail(string userId);
    }
}
