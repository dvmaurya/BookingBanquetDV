using CommonServices.ConstantData;
using CommonServices.Extensions;
using CommonServices.Model;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewModelsAndValidation;
using ViewModelsAndValidation.Command;
using ViewModelsAndValidation.Services;
using ViewModelsAndValidation.ViewModels;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace BookingBanquetDV.Source.Camera
{
    public class CameraViewModel : ViewModelBase, ICameraViewModel
    {
        private readonly ICommand _cameraClickCommand;
        private readonly ICommand _previewPhotoCommand;

        private readonly IMessageDialogService _messageDialogService;
        private StorageFile _storeFile;
        private PhotoDetails _photoDetails;

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

        public async Task<PhotoDetails> OnClickTakeAPhotoCommandAsync()
        {
            _storeFile = await CameraUtility.OpenCameraAndCapturePhotoAsync();

            if (_storeFile != null)
            {
                ulong photoSize = await GetCapturedPhotoSizeAsync(_storeFile);
                _photoDetails = new PhotoDetails
                {
                    StorageFile = _storeFile,
                    Date = _storeFile.DateCreated.ConvertToString(
                        ApplicationConstants.DatabaseDateTimeFormat),
                    FileType = _storeFile.FileType,
                    Size = photoSize,
                    ImageSource = await PhotoUtility.GetBitmapImageAsync(_storeFile),
                    PhotoGuid = Guid.NewGuid().ToString().ToLower(),
                };

                _photoDetails.LocalPhotoName = _photoDetails.PhotoGuid.ConcatString(ApplicationConstants.FileFormat);

                await PhotoUtility.SavePhotoAsync(
                    _photoDetails.StorageFile,
                    _photoDetails.LocalPhotoName);
            }

            return _photoDetails;
        }

        private async Task<ulong> GetCapturedPhotoSizeAsync(StorageFile storeFile)
        {
            BasicProperties basicProperties = await _storeFile.GetBasicPropertiesAsync();
            return basicProperties.Size;
        }

        public ICommand CameraClickCommand => _cameraClickCommand;

    }
}
