using BookingBanquetDV.Source.Camera;
using CommonServices.ConstantData;
using CommonServices.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewModelsAndValidation.Command;
using ViewModelsAndValidation.Core;
using ViewModelsAndValidation.ViewModels;
using Windows.Storage;

namespace BookingBanquetDV.Source.Welcome.Controls
{
    public class AllProfilePhotoViewModel : NotificationObject, IAllProfilePhotoViewModel
    {
        private IReadOnlyCollection<PhotoDetails> _photoDetailList;
        private string _photoCapturedOn;
        private PhotoDetails _selectedPhotoDetail;
        private readonly ICommand _onDialogLoadingCommand;
        private readonly ICommand _changePhotoCommand;

        public AllProfilePhotoViewModel()
        {
            _onDialogLoadingCommand = new DelegateCommand(
                parameter => OnDialogLoadAsync());

            _changePhotoCommand = new DelegateCommand(
                parameter => OnChangePhoto(parameter));
        }

        private void OnChangePhoto(object parameter)
        {
            if (parameter is PhotoDetails photoDetails)
                _selectedPhotoDetail = photoDetails;
        }

        private async Task OnDialogLoadAsync()
        {
            if (_photoDetailList != null)
            {
                await SetPhotoAsync(_photoDetailList);

                if (_selectedPhotoDetail != null)
                {
                    PhotoDetails checkedPhoto = _photoDetailList.FirstOrDefault(name =>
                    name.LocalPhotoName == _selectedPhotoDetail.LocalPhotoName);

                    if (checkedPhoto != null)
                        checkedPhoto.IsChecked = true;
                }

                OnPropertyChanged(() => PhotoDetailList);
            }
        }

        private async Task SetPhotoAsync(IReadOnlyCollection<PhotoDetails> photoDetailList)
        {
            StorageFolder storageFolder = await ApplicationData.Current.LocalFolder.
                    GetFolderAsync(ApplicationConstants.Document);

            foreach (PhotoDetails photo in photoDetailList)
            {
                if (await storageFolder.TryGetItemAsync(photo.LocalPhotoName) is StorageFile file)
                    photo.ImageSource = await PhotoUtility.GetDecrtypedPhotoAsync(file);
            }
        }

        public IReadOnlyCollection<PhotoDetails> PhotoDetailList
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

        public ICommand IsCheckedCommand => _changePhotoCommand;

        public PhotoDetails SelectedPhotoDetail
        {
            get => _selectedPhotoDetail;
            set => _selectedPhotoDetail = value;
        }
    }
}
