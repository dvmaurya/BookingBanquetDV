using CommonServices.Model;

namespace BookingServices.Interfaces
{
    public interface IBanquetDataProvider
    {
        void AddOrUpdateBanquet(BanquetDetail banquetDetail);
        BanquetDetail GetBanquetDetail();
    }
}
