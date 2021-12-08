using CommonServices.Model;
using System.Collections.Generic;
using System.Windows.Input;

namespace ViewModelsAndValidation.ViewModels
{
    public interface IAllProfilePhotoViewModel
    {
        IReadOnlyCollection<PhotoDetails> PhotoDetailList { get; set; }
        string PhotoCapturedOn { get; set; }
        ICommand OnDialogLoadingCommand { get; }
        ICommand IsCheckedCommand { get; }
        PhotoDetails SelectedPhotoDetail { get; set; }
    }
}
