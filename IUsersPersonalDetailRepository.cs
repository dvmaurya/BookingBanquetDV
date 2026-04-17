using CommonServices.Model.DbModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingServices.Repository.Interface
{
    public interface IUsersPersonalDetailRepository
    {
        bool AddOrUpdateUsersPersonalDetail(UsersPersonalDetail usersPersonalDetail);
        UsersPersonalDetail GetUsersPersonalDetail(string emailId);
    }
}
