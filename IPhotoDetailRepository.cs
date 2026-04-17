using CommonServices.Model.DbModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingServices.Repository.Interface
{
    public interface IPhotoDetailRepository
    {
        void AddOrUpdatePhotoDetail(PhotoDetails photoDetail);
        IList<PhotoDetails> GetAllPhotoDetail(string emailId);
        PhotoDetails GetActivePhotoDetail(string emailId);
        Task DeleteDocumentFileFromFolderAsync(string photoName);
    }
}
