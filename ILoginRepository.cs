using CommonServices.Model.DbModel;
using System.Collections.Generic;

namespace BookingServices.Repository.Interface
{
    public interface ILoginRepository
    {
        bool AddOrUpdateLoginDetail(LoginDetail loginDetail);
        IDictionary<string, string> GetDataForLogin(string userName, string password);
        IDictionary<string, string> GetPreviousLoginDetail();
    }
}
