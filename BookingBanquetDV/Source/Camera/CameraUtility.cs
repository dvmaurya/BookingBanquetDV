using CommonServices.ConstantData;
using System.Threading.Tasks;
using ViewModelsAndValidation;
using ViewModelsAndValidation.Services;
using Windows.Devices.Enumeration;
using Windows.Media.Capture;
using Windows.Storage;
using System;
using CommonServices.Logger;
using CommonServices.Extensions;

namespace BookingBanquetDV.Source.Camera
{
    public static class CameraUtility
    {
        private static readonly IMessageDialogService _messageDialogService =
            DIContainer.GetInstance<IMessageDialogService>();

        public static async Task<StorageFile> OpenCameraAndCapturePhotoAsync()
        {
            StorageFile storageFile = null;

            try
            {
                CameraCaptureUI cameraCapture = new CameraCaptureUI();
                cameraCapture.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
                cameraCapture.PhotoSettings.AllowCropping = false;
                cameraCapture.PhotoSettings.MaxResolution = CameraCaptureUIMaxPhotoResolution.MediumXga;

                storageFile = await cameraCapture.CaptureFileAsync(CameraCaptureUIMode.Photo);
            }
            catch (Exception ex)
            {
                await _messageDialogService.ShowMessageDialogAsync(
                    title: string.Empty,
                    content: ApplicationConstants.UnableToOpenCamera);

                FileLogger.Logger.Info("Exception: ".ConcatString(ex.Message));

                storageFile = null;
            }

            return storageFile;
        }

        public static async Task<bool> IsDeviceHasCameraAsync()
        {
            DeviceInformationCollection cameraDevice =
                await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);

            if (cameraDevice.Count > 0)
                return true;
            else
                return false;
        }
    }
}
