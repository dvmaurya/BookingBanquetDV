using CommonServices.Model;
using System.Threading.Tasks;

namespace BookingServices.Interfaces
{
    public interface ICustomerPhotoDataProvider
    {
        void AddOrUpdatePhotoDetail(CustomerPhotoDetail photoDetail);
        CustomerPhotoDetail GetCustomerPhotoDetail(int bookingId);
        Task DeleteCustomerPhotoAsync(CustomerPhotoDetail photoDetail);
    }
}
