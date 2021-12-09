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
        ICommand ChangePhotoCommand { get; }
        PhotoDetails SelectedPhotoDetail { get; set; }
    }
}
