using CommonServices.Model;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewModelsAndValidation;
using ViewModelsAndValidation.Command;
using ViewModelsAndValidation.Services;
using ViewModelsAndValidation.ViewModels;
using Windows.Storage;

namespace BookingBanquetDV.Source.Camera
{
    public class CameraViewModel : ViewModelBase, ICameraViewModel
    {
        private readonly ICommand _cameraClickCommand;
        private readonly ICommand _previewPhotoCommand;

        private readonly IMessageDialogService _messageDialogService;
        private StorageFile _storeFile;
        
        public CameraViewModel(
            IMessageDialogService messageDialogService)
        {
            _messageDialogService = messageDialogService ?? throw new ArgumentNullException("messageDialogService");

            _cameraClickCommand = new DelegateCommand(async camera => await OnClickTakeAPhotoCommandAsync());
            _previewPhotoCommand = new DelegateCommand(async camera => await OnPreviewPhotoAsync(camera));

        }

        private async Task OnPreviewPhotoAsync(object viewPhoto)
        {
            if (viewPhoto != null && viewPhoto is PhotoDetails photoDetails)
                await ShowPreviewPhotoDialogAsync(photoDetails);
        }

        private Task ShowPreviewPhotoDialogAsync(PhotoDetails photoDetails)
        {
            throw new NotImplementedException();
        }

        private async Task OnClickTakeAPhotoCommandAsync()
        {
            _storeFile = await CameraUtility.OpenCameraAndCapturePhotoAsync();

            if(_storeFile != null)
            {
                PhotoDetails photoDetails = new PhotoDetails
                {
                    StorageFile = _storeFile,
                    ImageSource = await PhotoUtility.GetBitmapImageAsync(_storeFile),
                    PhotoGuid = Guid.NewGuid().ToString().ToLower()
            };

                //PreviewPhoto previewPhoto = new PreviewPhoto(photoDetails);
                //previewPhoto.PhotoCaptured +=
                //    async (sender, eventArgs) => await OnPhotoCapturedCompleted(sender, eventArgs);
                await PhotoUtility.SavePhotoAsync(
                    photoDetails.StorageFile,
                    photoDetails.LocalPhotoName);
            }
        }

        public ICommand CameraClickCommand => _cameraClickCommand;

    }
}
