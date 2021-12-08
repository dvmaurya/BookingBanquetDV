using CommonServices.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingServices.Interfaces
{
    public interface IUsersPersonalDetailDataProvider
    {
        bool AddOrUpdateUsersPersonalDetail(UsersPersonalDetail usersPersonalDetail);
        UsersPersonalDetail GetUsersPersonalDetail(string emailId);
        Task DeletePhotoFromFolderAsync(string photoName);
    }
}
