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
        private CustomerPhotoDetail _customerPhoto;
        private readonly ICommand _onLoadDialogCommand;

        public ViewBookingViewModel()
        {
            _onLoadDialogCommand = new DelegateCommand(
                async parameter => await OnDialogLoadAsync());
        }

        private async Task OnDialogLoadAsync()
        {
            if (_bookingDetail != null)
                await SetPhotoAsync();
        }

        private async Task SetPhotoAsync()
        {
            if (_customerPhoto != null && !string.IsNullOrEmpty(_customerPhoto.PhotoName))
            {
                StorageFolder storageFolder = await ApplicationData.Current.LocalFolder.
                    GetFolderAsync(ApplicationConstants.CustomerPhoto);

                if (await storageFolder.TryGetItemAsync(_customerPhoto.PhotoName) is StorageFile file)
                    _customerPhoto.ImageSource = await PhotoUtility.GetDecrtypedPhotoAsync(file, ApplicationConstants.CustomerPhoto);

                OnPropertyChanged(() => CustomerPhoto);
            }
        }

        public BookingDetails BookingDetails
        {
            get => _bookingDetail;
            set => _bookingDetail = value;
        }

        public ICommand OnLoadDialogCommand => _onLoadDialogCommand;

        public CustomerPhotoDetail CustomerPhoto
        {
            get => _customerPhoto;
            set => _customerPhoto = value;
        }
    }
}
