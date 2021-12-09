using System;
using System.Threading.Tasks;
using System.Windows.Input;
using BookingBanquetDV.Source.Camera;
using CommonServices.ConstantData;
using CommonServices.Model;
using ViewModelsAndValidation.Command;
using ViewModelsAndValidation.Core;
using ViewModelsAndValidation.ViewModels;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace BookingBanquetDV.Source.Welcome.Controls
{
    public class ViewBookingViewModel : NotificationObject, IViewBookingViewModel
    {
        private BookingDetails _bookingDetail;
        private BitmapImage _imageSource;
        private readonly ICommand _onLoadDialogCommand;

        public ViewBookingViewModel()
        {
            _onLoadDialogCommand = new DelegateCommand(
                async parameter => await OnDialogLoadAsync());
        }

        private async Task OnDialogLoadAsync()
        {
            if(_bookingDetail != null)
                await SetPhotoAsync(_bookingDetail.PhotoName);
        }

        private async Task SetPhotoAsync(string photoName)
        {
            if (!string.IsNullOrEmpty(photoName))
            {
                StorageFolder storageFolder = await ApplicationData.Current.LocalFolder.
                    GetFolderAsync(ApplicationConstants.Document);

                if (await storageFolder.TryGetItemAsync(photoName) is StorageFile file)
                    _imageSource = await PhotoUtility.GetDecrtypedPhotoAsync(file);

                OnPropertyChanged(() => ImageSource);
            }
        }

        public BookingDetails BookingDetails
        {
            get => _bookingDetail;
            set => _bookingDetail = value;
        }

        public ICommand OnLoadDialogCommand => _onLoadDialogCommand;

        public BitmapImage ImageSource
        {
            get => _imageSource;
            set => _imageSource = value;
        }
    }
}
