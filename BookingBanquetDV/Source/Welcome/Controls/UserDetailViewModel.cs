using BookingBanquetDV.Source.Camera;
using CommonServices.ConstantData;
using CommonServices.Model;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewModelsAndValidation.Command;
using ViewModelsAndValidation.ViewModels;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace BookingBanquetDV.Source.Welcome.Controls
{
    public class UserDetailViewModel : NotificationModel, IUserDetailViewModel
    {
        private Users _users;
        private BitmapImage _imageSource;
        private readonly ICommand _onDialogLoadCommand;

        public UserDetailViewModel()
        {
            _onDialogLoadCommand = new DelegateCommand(
                async parameter => await OnDialogLoadAsync());
        }

        private async Task OnDialogLoadAsync()
        {
            if (_users != null)
            {
                StorageFolder storageFolder = await ApplicationData.Current.LocalFolder.
                    GetFolderAsync(ApplicationConstants.Document);

                if (await storageFolder.TryGetItemAsync(_users.PhotoName) is StorageFile file)
                    _imageSource = await PhotoUtility.GetDecrtypedPhotoAsync(file);
            }
        }

        public ICommand OnDialogLoadCommand => _onDialogLoadCommand;

        public Users Users
        {
            get => _users;
            set => _users = value;
        }

        public BitmapImage ImageSource
        {
            get => _imageSource;
            set
            {
                _imageSource = value;
                OnPropertyChanged("ImageSource");
            }
        }
    }
}
