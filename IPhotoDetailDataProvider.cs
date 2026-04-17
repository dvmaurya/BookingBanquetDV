using CommonServices.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingServices.Interfaces
{
    public interface IPhotoDetailDataProvider
    {
        void AddOrUpdatePhotoDetail(PhotoDetails photoDetail);
        IList<PhotoDetails> GetAllPhotoDetail(string emailId);
        PhotoDetails GetActivePhotoDetail(string emailId);
        Task DeleteDocumentFileFromFolderAsync(string photoName);
    }
}
