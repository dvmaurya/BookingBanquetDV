using CommonServices.ConstantData;
using CommonServices.Model;
using System;
using DbModel = CommonServices.Model.DbModel;

namespace CommonServices.Extensions
{
    public static class BanquetExtension
    {
        public static DbModel.BanquetDetail ToDbBanquetModel(this BanquetDetail banquetModel)
        {
            DbModel.BanquetDetail dbBanquetModel = new DbModel.BanquetDetail
            {
                Id = banquetModel.Id,
                AvailableFlours = banquetModel.AvailableFlours,
                AvailableMandap = banquetModel.AvailableMandap,
                AvailableParkings = banquetModel.AvailableParkings,
                AvailableRooms = banquetModel.AvailableRooms,
                EmailId = banquetModel.EmailId,
                UpdationDate = DateTimeOffset.Now.Date.ConvertToString(ApplicationConstants.DatabaseDateFormat)
            };

            return dbBanquetModel;
        }

        public static BanquetDetail ToBanquetModel(this DbModel.BanquetDetail DbBanquetModel)
        {
            BanquetDetail banquetModel = new BanquetDetail
            {
                Id = DbBanquetModel.Id,
                AvailableFlours = DbBanquetModel.AvailableFlours,
                AvailableMandap = DbBanquetModel.AvailableMandap,
                AvailableParkings = DbBanquetModel.AvailableParkings,
                AvailableRooms = DbBanquetModel.AvailableRooms,
                EmailId = DbBanquetModel.EmailId,
                UpdationDate = DbBanquetModel.UpdationDate
            };

            return banquetModel;
        }
    }
}
