using CommonServices.Model;
using DbModel = CommonServices.Model.DbModel;

namespace CommonServices.Extensions
{
    public static class PhotoExtension
    {
        public static DbModel.PhotoDetails ToDbPhotoDetail(this PhotoDetails photoDetails)
        {
            DbModel.PhotoDetails dbPhotoDetails = new DbModel.PhotoDetails
            {
                Id = photoDetails.Id,
                PhotoName = photoDetails.LocalPhotoName,
                Date = photoDetails.Date,
                FileType = photoDetails.FileType,
                Size = photoDetails.Size,
                EmailId = photoDetails.EmailId,
                IsPhotoActive = photoDetails.IsPhotoActive,
                IsPhotoRemoved = photoDetails.IsPhotoRemoved
            };

            return dbPhotoDetails;
        }

        public static PhotoDetails ToPhotoDetails(this DbModel.PhotoDetails dbPhotoDetails)
        {
            PhotoDetails photoDetail = new PhotoDetails
            {
                Id = dbPhotoDetails.Id,
                LocalPhotoName = dbPhotoDetails.PhotoName,
                Date = dbPhotoDetails.Date,
                FileType = dbPhotoDetails.FileType,
                Size = dbPhotoDetails.Size,
                EmailId = dbPhotoDetails.EmailId,
                IsPhotoActive = dbPhotoDetails.IsPhotoActive,
                IsPhotoRemoved = dbPhotoDetails.IsPhotoRemoved
            };

            return photoDetail;
        }
    }
}
