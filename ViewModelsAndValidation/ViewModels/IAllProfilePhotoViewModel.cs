using CommonServices.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ViewModelsAndValidation.ViewModels
{
    public interface IAllProfilePhotoViewModel
    {
        ObservableCollection<PhotoDetails> PhotoDetailList { get; set; }
        string PhotoCapturedOn { get; set; }
        ICommand OnDialogLoadingCommand { get; }
        ICommand ChangePhotoCommand { get; }
        ICommand DeletePhotoCommand { get; }
        PhotoDetails SelectedPhotoDetail { get; set; }
    }
}
