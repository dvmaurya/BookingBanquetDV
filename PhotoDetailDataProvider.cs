using BookingServices.Interfaces;
using BookingServices.Repository.Interface;
using CommonServices.Extensions;
using CommonServices.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbModel = CommonServices.Model.DbModel;

namespace BookingServices.DataProvider
{
    public class PhotoDetailDataProvider : IPhotoDetailDataProvider
    {
        private readonly IPhotoDetailRepository _photoDetailRepository;

        public PhotoDetailDataProvider(
            IPhotoDetailRepository photoDetailRepository)
        {
            _photoDetailRepository = photoDetailRepository ?? throw new ArgumentNullException(nameof(photoDetailRepository));
        }

        public void AddOrUpdatePhotoDetail(PhotoDetails photoDetail)
        {
            if (photoDetail != null)
            {
                _photoDetailRepository.AddOrUpdatePhotoDetail(photoDetail.ToDbPhotoDetail());
            }
        }

        public IList<PhotoDetails> GetAllPhotoDetail(string emailId)
        {
            IList<PhotoDetails> photoDetails = new List<PhotoDetails>();

            IList<DbModel.PhotoDetails> dbPhotoDetail = _photoDetailRepository.GetAllPhotoDetail(emailId);

            if(dbPhotoDetail != null && dbPhotoDetail.Any())
            {
                foreach (DbModel.PhotoDetails photoDetail in dbPhotoDetail)
                    photoDetails.Add(photoDetail.ToPhotoDetails());
            }

            return photoDetails;
        }

        public PhotoDetails GetActivePhotoDetail(string emailId)
        {
            DbModel.PhotoDetails photoDetail = _photoDetailRepository.GetActivePhotoDetail(emailId);

            if (photoDetail != null)
                return photoDetail.ToPhotoDetails();

            return null;
        }

        public async Task DeleteDocumentFileFromFolderAsync(string photoName)
        {
            await _photoDetailRepository.DeleteDocumentFileFromFolderAsync(photoName).ConfigureAwait(true);
        }
    }
}
