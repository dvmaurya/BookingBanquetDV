using CommonServices.Model;
using DbModel = CommonServices.Model.DbModel;

namespace CommonServices.Extensions
{
    public static class CustomerPhotoEtension
    {
        public static DbModel.CustomerPhotoDetail ToDbPhotoDetail(this CustomerPhotoDetail photoDetails)
        {
            DbModel.CustomerPhotoDetail dbPhotoDetails = new DbModel.CustomerPhotoDetail
            {
                Id = photoDetails.Id,
                PhotoName = photoDetails.PhotoName,
                Date = photoDetails.Date,
                FileType = photoDetails.FileType,
                Size = photoDetails.Size,
                EmailId = photoDetails.EmailId,
                BookingId = photoDetails.BookingId
            };

            return dbPhotoDetails;
        }

        public static CustomerPhotoDetail ToPhotoDetails(this DbModel.CustomerPhotoDetail dbPhotoDetails)
        {
            CustomerPhotoDetail photoDetail = new CustomerPhotoDetail
            {
                Id = dbPhotoDetails.Id,
                PhotoName = dbPhotoDetails.PhotoName,
                Date = dbPhotoDetails.Date,
                FileType = dbPhotoDetails.FileType,
                Size = dbPhotoDetails.Size,
                EmailId = dbPhotoDetails.EmailId,
                BookingId = dbPhotoDetails.BookingId,
            };

            return photoDetail;
        }
    }
}
