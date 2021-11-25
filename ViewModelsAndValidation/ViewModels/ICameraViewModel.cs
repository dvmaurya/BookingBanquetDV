using CommonServices.Model;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModelsAndValidation.ViewModels
{
    public interface ICameraViewModel
    {
        ICommand CameraClickCommand { get; }
        Task<PhotoDetails> OnClickTakeAPhotoCommandAsync();
    }
}
