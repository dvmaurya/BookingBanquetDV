using CommonServices.Model;
using System.Threading.Tasks;

namespace ViewModelsAndValidation.ViewModels
{
    public interface ICameraViewModel
    {
        Task<PhotoDetails> OnClickTakeAPhotoAsync(string folderName);
        Task<CustomerPhotoDetail> OnClickCustomerPhotoAsync(string folderName);
    }
}
