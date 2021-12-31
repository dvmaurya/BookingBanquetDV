using BookingBanquetDV.Source.Camera;
using CommonServices.ConstantData;
using CommonServices.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewModelsAndValidation.Command;
using ViewModelsAndValidation.Core;
using ViewModelsAndValidation.Services;
using ViewModelsAndValidation.ViewModels;
using Windows.Storage;
using Windows.UI.Popups;

namespace BookingBanquetDV.Source.Welcome.Controls
{
    public class AllProfilePhotoViewModel : NotificationObject, IAllProfilePhotoViewModel
    {
        private ObservableCollection<PhotoDetails> _photoDetailList;
        private string _photoCapturedOn;
        private PhotoDetails _selectedPhotoDetail;
        private readonly ICommand _onDialogLoadingCommand;
        private readonly ICommand _changePhotoCommand;
        private readonly ICommand _deletePhotoCommand;

        private readonly IMessageDialogService _messageDialogService;

        public AllProfilePhotoViewModel(
            IMessageDialogService messageDialogService)
        {
            _messageDialogService = messageDialogService ?? throw new ArgumentNullException("messageDialogService");

            _onDialogLoadingCommand = new DelegateCommand(
                async parameter => await OnDialogLoadAsync());

            _changePhotoCommand = new DelegateCommand(
                parameter => OnChangePhoto(parameter));

            _deletePhotoCommand = new DelegateCommand(
                async parameter => await OnDeletePhotoAsync(parameter));
        }

        private async Task OnDeletePhotoAsync(object parameter)
        {
            List<IUICommand> commands = new List<IUICommand>
            {
                new UICommand(
                    label: ApplicationConstants.Yes,
                    action: (uiCommand) => RemovePhotoContinueAsync(parameter)),

                new UICommand(
                    label: ApplicationConstants.No,
                    action: null)
            };

            await _messageDialogService.ShowMessageDialogAsync(
                ApplicationConstants.Warning,
                ApplicationConstants.RemovePhotoWarning,
                commands);
        }

        private void RemovePhotoContinueAsync(object parameter)
        {
            if (parameter is PhotoDetails photoDetails)
            {
                photoDetails.IsChecked = false;
                photoDetails.IsPhotoRemoved = true;
                _selectedPhotoDetail = photoDetails;

                _photoDetailList.Remove(photoDetails);

                OnPropertyChanged(() => PhotoDetailList);
            }
        }

        private void OnChangePhoto(object parameter)
        {
            if (parameter is PhotoDetails photoDetails)
            {
                _selectedPhotoDetail = photoDetails;

                OnPropertyChanged(() => PhotoDetailList);
            }
        }

        private async Task OnDialogLoadAsync()
        {
            if (_photoDetailList != null)
            {
                await SetPhotoAsync(_photoDetailList);

                OnPropertyChanged(() => PhotoDetailList);
            }
        }

        private async Task SetPhotoAsync(IList<PhotoDetails> photoDetailList)
        {
            if (photoDetailList != null && photoDetailList.Any())
            {
                StorageFolder storageFolder = await ApplicationData.Current.LocalFolder.
                        GetFolderAsync(ApplicationConstants.Document);

                foreach (PhotoDetails photo in photoDetailList)
                {
                    if (await storageFolder.TryGetItemAsync(photo.LocalPhotoName) is StorageFile file)
                        photo.ImageSource = await PhotoUtility.GetDecrtypedPhotoAsync(file, ApplicationConstants.Document);
                }
            }
        }

        public ObservableCollection<PhotoDetails> PhotoDetailList
        {
            get => _photoDetailList;
            set => _photoDetailList = value;
        }

        public string PhotoCapturedOn
        {
            get => _photoCapturedOn;
            set => _photoCapturedOn = value;
        }

        public ICommand OnDialogLoadingCommand => _onDialogLoadingCommand;

        public ICommand ChangePhotoCommand => _changePhotoCommand;

        public ICommand DeletePhotoCommand => _deletePhotoCommand;

        public PhotoDetails SelectedPhotoDetail
        {
            get => _selectedPhotoDetail;
            set => _selectedPhotoDetail = value;
        }
    }
}
