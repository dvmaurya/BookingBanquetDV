using BookingServices.Interfaces;
using BookingServices.Repository.Interface;
using CommonServices.Extensions;
using CommonServices.Model;
using System;
using DbModel = CommonServices.Model.DbModel;


namespace BookingServices.DataProvider
{
    public class BanquetDataProvider : IBanquetDataProvider
    {
        private readonly IBanquetRepository _banquetRepository;

        public BanquetDataProvider(
            IBanquetRepository banquetRepository)
        {
            _banquetRepository = banquetRepository ?? throw new ArgumentNullException(nameof(banquetRepository));
        }

        public void AddOrUpdateBanquet(BanquetDetail banquetDetail)
        {
            _banquetRepository.AddOrUpdateBanquet(banquetDetail.ToDbBanquetModel());
        }

        public BanquetDetail GetBanquetDetail()
        {
            DbModel.BanquetDetail banquetDetail = _banquetRepository.GetBanquetDetail();

            if (banquetDetail != null)
                return banquetDetail.ToBanquetModel();

            return null;
        }
    }
}
