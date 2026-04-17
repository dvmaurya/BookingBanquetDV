using CommonServices.Model;

namespace BookingServices.Interfaces
{
    public interface IUsersPersonalDetailDataProvider
    {
        bool AddOrUpdateUsersPersonalDetail(UsersPersonalDetail usersPersonalDetail);
        UsersPersonalDetail GetUsersPersonalDetail(string emailId);
    }
}
