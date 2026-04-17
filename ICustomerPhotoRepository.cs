using CommonServices.Model.DbModel;
using System.Threading.Tasks;

namespace BookingServices.Repository.Interface
{
    public interface ICustomerPhotoRepository
    {
        void AddOrUpdatePhotoDetail(CustomerPhotoDetail photoDetail);
        CustomerPhotoDetail GetCustomerPhotoDetail(int bookingId);
        Task DeleteCustomerPhotoAsync(CustomerPhotoDetail photoDetail);
    }
}
