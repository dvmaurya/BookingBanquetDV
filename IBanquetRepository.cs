using CommonServices.Model.DbModel;

namespace BookingServices.Repository.Interface
{
    public interface IBanquetRepository
    {
        void AddOrUpdateBanquet(BanquetDetail banquetDetail);
        BanquetDetail GetBanquetDetail();
    }
}
