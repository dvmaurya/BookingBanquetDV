using System.Windows.Input;

namespace ViewModelsAndValidation.ViewModels
{
    public interface ICameraViewModel
    {
        ICommand CameraClickCommand { get; }
    }
}
