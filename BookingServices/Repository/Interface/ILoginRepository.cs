using CommonServices.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingServices.Repository.Interface
{
    public interface ILoginRepository
    {
        bool AddOrUpdateLoginDetail(LoginDetail loginDetail);
        IDictionary<string, string> GetDataForLogin(string userName, string password);
        IDictionary<string, string> GetPreviousLoginDetail();
    }
}
