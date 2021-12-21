using CommonServices.ConstantData;
using CommonServices.Extensions;
using CommonServices.Model;
using System;
using System.Threading.Tasks;
using ViewModelsAndValidation;
using ViewModelsAndValidation.Services;
using ViewModelsAndValidation.ViewModels;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace BookingBanquetDV.Source.Camera
{
    public class CameraViewModel : ViewModelBase, ICameraViewModel
    {
        private readonly IMessageDialogService _messageDialogService;
        private StorageFile _storeFile;
        private CustomerPhotoDetail _cutomerPhotoDetails;
        private PhotoDetails _photoDetails;

        public CameraViewModel(
            IMessageDialogService messageDialogService)
        {
            _messageDialogService = messageDialogService ?? throw new ArgumentNullException("messageDialogService");
        }

        public async Task<CustomerPhotoDetail> OnClickCustomerPhotoAsync(string folderName)
        {
            _storeFile = await CameraUtility.OpenCameraAndCapturePhotoAsync();

            if (_storeFile != null)
            {
                ulong photoSize = await GetCapturedPhotoSizeAsync(_storeFile);
                _cutomerPhotoDetails = new CustomerPhotoDetail
                {
                    StorageFile = _storeFile,
                    Date = _storeFile.DateCreated.ConvertToString(
                        ApplicationConstants.DatabaseDateTimeFormat),
                    FileType = _storeFile.FileType,
                    Size = photoSize,
                    ImageSource = await PhotoUtility.GetBitmapImageAsync(_storeFile),
                    PhotoGuid = Guid.NewGuid().ToString().ToLower()
                };

                _cutomerPhotoDetails.PhotoName = _cutomerPhotoDetails.PhotoGuid.
                    ConcatString(ApplicationConstants.FileFormat);

                await PhotoUtility.SavePhotoAsync(
                    _cutomerPhotoDetails.StorageFile,
                    _cutomerPhotoDetails.PhotoName,
                    folderName);
            }

            return _cutomerPhotoDetails;
        }

        public async Task<PhotoDetails> OnClickTakeAPhotoAsync(string folderName)
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
                    _photoDetails.LocalPhotoName,
                    folderName);
            }

            return _photoDetails;
        }

        private async Task<ulong> GetCapturedPhotoSizeAsync(StorageFile storeFile)
        {
            BasicProperties basicProperties = await _storeFile.GetBasicPropertiesAsync();
            return basicProperties.Size;
        }

    }
}
