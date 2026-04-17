using System;
using System.Threading.Tasks;
using BookingServices.Interfaces;
using BookingServices.Repository.Interface;
using CommonServices.Extensions;
using CommonServices.Model;
using DbModel = CommonServices.Model.DbModel;

namespace BookingServices.DataProvider
{
    public class CustomerPhotoDataProvider : ICustomerPhotoDataProvider
    {
        private readonly ICustomerPhotoRepository _customerPhotoRepository;

        public CustomerPhotoDataProvider(
            ICustomerPhotoRepository customerPhotoRepository)
        {
            _customerPhotoRepository = customerPhotoRepository ?? throw new ArgumentNullException(nameof(customerPhotoRepository));
        }

        public void AddOrUpdatePhotoDetail(CustomerPhotoDetail photoDetail)
        {
            _customerPhotoRepository.AddOrUpdatePhotoDetail(photoDetail.ToDbPhotoDetail());
        }

        public async Task DeleteCustomerPhotoAsync(CustomerPhotoDetail photoDetail)
        {
            await _customerPhotoRepository.DeleteCustomerPhotoAsync(photoDetail.ToDbPhotoDetail()).ConfigureAwait(true);
        }

        public CustomerPhotoDetail GetCustomerPhotoDetail(int bookingId)
        {
            DbModel.CustomerPhotoDetail customerPhoto =_customerPhotoRepository.GetCustomerPhotoDetail(bookingId);

            if(customerPhoto != null)
                return customerPhoto.ToPhotoDetails();

            return null;
        }
    }
}
