using BookingBanquetDV.Converters;
using BookingBanquetDV.Source.Camera;
using BookingBanquetDV.Source.Register;
using BookingBanquetDV.Utility;
using BookingBanquetDV.Validation;
using BookingServices.Interfaces;
using CommonServices.ConstantData;
using CommonServices.Extensions;
using CommonServices.Model;
using CommonServices.Model.City;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewModelsAndValidation;
using ViewModelsAndValidation.Command;
using ViewModelsAndValidation.Services;
using ViewModelsAndValidation.ViewModels;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace BookingBanquetDV.Source.Welcome.Controls
{
    public class UserDetailViewModel : ValidationViewModelBase, IUserDetailViewModel, INotesValidator
    {
        private UsersPersonalDetail _usersPersonalDetail;
        private PhotoDetails _originalPhotoDetail;
        private string _fullName;
        private string _address;
        private string _photoName;
        private BitmapImage _imageSource;
        private IList<PhotoDetails> _removePhoto;
        private List<SingleSelection> _countryList;
        private List<SingleSelection> _stateList;
        private List<SingleSelection> _cityList;
        private SingleSelection _selectedCountry;
        private SingleSelection _selectedState;
        private SingleSelection _selectedCity;
        private ReadOnlyCollection<Country> _masterCountryList;
        private List<State> _masterStateList;
        private bool _isStateVisible;
        private bool _isCityVisible;
        private ICameraViewModel _cameraViewModel;
        private bool _isPreviousButtonVisible;
        private bool _isRemoveButtonVisible;
        private PhotoDetails _newPhotoDetails;
        private bool _isActivePhotoUpdate;
        private IList<PhotoDetails> _allPhotoDetails;
        private readonly IMessageDialogService _messageDialogService;
        private readonly ICommand _previousPhotoCommand;
        private readonly ICommand _removePhotoCommand;
        private readonly ICommand _retakePhotoCommand;
        private readonly ICommand _saveUserDetailCommand;
        private readonly ICommand _changedCountryCommand;
        private readonly ICommand _changedStateCommand;
        private readonly ICommand _changedCityCommand;

        private readonly ICountryDataProvider _countryDataProvider;
        private readonly IUsersPersonalDetailDataProvider _usersPersonalDetailDataProvider;
        private readonly INavigationService _navigationService;
        private readonly IPhotoDetailDataProvider _photoDetailDataProvider;
        private readonly IContentDialogService _contentDialogService;

        public UserDetailViewModel(
            ICountryDataProvider countryDataProvider,
            IUsersPersonalDetailDataProvider usersPersonalDetailDataProvider,
            INavigationService navigationService,
            ICameraViewModel cameraViewModel,
            IMessageDialogService messageDialogService,
            IPhotoDetailDataProvider photoDetailDataProvider,
            IContentDialogService contentDialogService)
        {
            _countryDataProvider = countryDataProvider ?? throw new ArgumentNullException("countryDataProvider");
            _usersPersonalDetailDataProvider = usersPersonalDetailDataProvider ?? throw new ArgumentNullException("usersPersonalDetailDataProvider");
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");
            _cameraViewModel = cameraViewModel ?? throw new ArgumentNullException("cameraViewModel");
            _messageDialogService = messageDialogService ?? throw new ArgumentNullException("messageDialogService");
            _photoDetailDataProvider = photoDetailDataProvider ?? throw new ArgumentNullException("photoDetailDataProvider");
            _contentDialogService = contentDialogService ?? throw new ArgumentNullException("contentDialogService");

            _removePhoto = new List<PhotoDetails>();

            _saveUserDetailCommand = new DelegateCommand(
                async parameter => await OnSaveUserDetailAsync());

            _changedCountryCommand = new DelegateCommand(
                async parameter => await OnCountryChangedAsync());

            _changedStateCommand = new DelegateCommand(
                async parameter => await OnStateChangedAsync());

            _changedCityCommand = new DelegateCommand(
                async parameter => await OnCityChangedAsync());

            _retakePhotoCommand = new DelegateCommand(
                async parameter => await OnRetakePhotoAsync());

            _previousPhotoCommand = new DelegateCommand(
                async parameter => await OnPreviousPhotoAsync());

            _removePhotoCommand = new DelegateCommand(
                async parameter => await OnRemovePhotoAsync(parameter));
        }

        private async Task OnRemovePhotoAsync(object parameter)
        {
            List<IUICommand> commands = new List<IUICommand>
            {
                new UICommand(
                    label: ApplicationConstants.Yes,
                    action: async(uiCommand) => await RemovePhotoContinueAsync(parameter)),

                new UICommand(
                    label: ApplicationConstants.No,
                    action: null)
            };

            await _messageDialogService.ShowMessageDialogAsync(
                ApplicationConstants.Warning,
                ApplicationConstants.RemovePhotoWarning,
                commands);
        }

        private async Task RemovePhotoContinueAsync(object parameter)
        {
            if (_originalPhotoDetail != null &&
                _originalPhotoDetail.LocalPhotoName.FormatStringEquals(_photoName))
            {
                await _messageDialogService.ShowMessageDialogAsync(
                    ApplicationConstants.Note,
                    ApplicationConstants.PhotoRemoveMessage);

                _isActivePhotoUpdate = true;
                _originalPhotoDetail.IsChecked = false;
            }
            else if (_newPhotoDetails != null &&
                _newPhotoDetails.LocalPhotoName.FormatStringEquals(_photoName))
            {
                if (_newPhotoDetails.Id == 0)
                {
                    _removePhoto.Add(_newPhotoDetails);
                    _newPhotoDetails = null;
                }
                else
                {
                    _newPhotoDetails.IsChecked = false;
                    _photoName = string.Empty;
                }
            }

            _imageSource = null;
            OnPropertyChanged(() => ImageSource);
        }

        private async Task OnPreviousPhotoAsync()
        {
            if(_allPhotoDetails == null)
                _allPhotoDetails = _photoDetailDataProvider.GetAllPhotoDetail(
                    _usersPersonalDetail.EmailId);

            AllProfilePhoto allProfilePhoto = new AllProfilePhoto(_allPhotoDetails);
            await _contentDialogService.ShowDialogAsync(allProfilePhoto);

            PhotoDetails photoDetails = allProfilePhoto?.SelectedPhotoDetail;

            if (photoDetails != null)
            {
                if (!photoDetails.IsPhotoRemoved)
                {
                    if (_newPhotoDetails != null && _newPhotoDetails.Id == 0)
                        _removePhoto.Add(_newPhotoDetails);

                    if (_originalPhotoDetail != null &&
                        _originalPhotoDetail.LocalPhotoName !=
                        photoDetails.LocalPhotoName)
                    {
                        _originalPhotoDetail.IsChecked = false;
                        _isActivePhotoUpdate = true;
                        await SetNewPhotoDetails(photoDetails);
                    }
                    else
                    {
                        await SetNewPhotoDetails(photoDetails);
                    }
                }
                else
                {
                    if (_originalPhotoDetail != null)
                        _originalPhotoDetail.IsChecked = false;

                    if(_newPhotoDetails != null)
                        await SetPhotoAsync(_newPhotoDetails.LocalPhotoName);
                    else
                        _imageSource = null;

                    OnPropertyChanged(() => ImageSource);
                }
            }
        }

        private async Task SetNewPhotoDetails(PhotoDetails photoDetails)
        {
            _photoName = photoDetails.LocalPhotoName;
            _newPhotoDetails = photoDetails;

            await SetPhotoAsync(_newPhotoDetails.LocalPhotoName);
        }

        private async Task OnRetakePhotoAsync()
        {
            PhotoDetails photoDetails = await _cameraViewModel.OnClickTakeAPhotoAsync(ApplicationConstants.Document);

            if (photoDetails != null)
            {
                if (_newPhotoDetails != null && _newPhotoDetails.Id == 0)
                    _removePhoto.Add(_newPhotoDetails);

                _newPhotoDetails = photoDetails;
                _allPhotoDetails = null;
                _isActivePhotoUpdate = true;

                _photoName = _newPhotoDetails.LocalPhotoName;
                _newPhotoDetails.IsChecked = true;

                if (_originalPhotoDetail != null)
                    _originalPhotoDetail.IsChecked = false;

                if (_usersPersonalDetail != null)
                    _newPhotoDetails.EmailId = _usersPersonalDetail.EmailId;

                await SetPhotoAsync(_newPhotoDetails.LocalPhotoName);
            }
        }

        private async Task OnCityChangedAsync()
        {
            _selectedCity = await SingleSelectionUtility.OnSelectSingleSelectionAsync(
                        _cityList, _selectedCity, ApplicationConstants.City);

            OnPropertyChanged(() => SelectedCity);
        }

        private async Task OnStateChangedAsync()
        {
            _selectedState = await SingleSelectionUtility.OnSelectSingleSelectionAsync(
                        _stateList, _selectedState, ApplicationConstants.State);

            if (_selectedState != null)
            {
                _isCityVisible = true;

                IReadOnlyCollection<City> cityList =
                    JsonConvert.DeserializeObject<IReadOnlyCollection<City>>(_selectedState.JsonData);

                if (cityList != null)
                    GenerateListForSingleSelection(cityList);

                await OnCityChangedAsync();
            }

            OnPropertyChanged(() => IsCityVisible);
            OnPropertyChanged(() => SelectedState);
        }

        private async Task OnCountryChangedAsync()
        {
            _selectedCountry = await SingleSelectionUtility.OnSelectSingleSelectionAsync(
                        _countryList, _selectedCountry, ApplicationConstants.Country);

            if (_selectedCountry != null)
            {
                _isStateVisible = true;

                IReadOnlyCollection<State> stateList =
                    JsonConvert.DeserializeObject<IReadOnlyCollection<State>>(_selectedCountry.JsonData);

                if (stateList != null)
                    GenerateListForSingleSelection(stateList);

                await OnStateChangedAsync();
            }

            OnPropertyChanged(() => IsStateVisible);
            OnPropertyChanged(() => SelectedCountry);
        }

        private async Task OnSaveUserDetailAsync()
        {
            if (ValidateProperties())
            {
                if (string.IsNullOrEmpty(_photoName))
                {
                    List<IUICommand> commands = new List<IUICommand>
                    {
                        new UICommand(
                            label: ApplicationConstants.Yes,
                            action: (uiCommand) => OnContinueSaveAsync()),

                        new UICommand(
                            label: ApplicationConstants.No,
                            action: null)
                    };

                    await _messageDialogService.ShowMessageDialogAsync(
                        ApplicationConstants.Warning,
                        ApplicationConstants.PhotoNotAvailable.FormatString(ApplicationConstants.ChooseFromList),
                        commands);
                }
                else
                {
                    OnContinueSaveAsync();
                }
            }
        }

        private void OnContinueSaveAsync()
        {
            if (_usersPersonalDetail != null)
            {
                _usersPersonalDetail.FullName = _fullName;
                _usersPersonalDetail.Address = _address;
                _usersPersonalDetail.Country = _selectedCountry?.Name ?? string.Empty;
                _usersPersonalDetail.State = _selectedState?.Name ?? string.Empty;
                _usersPersonalDetail.City = _selectedCity?.Name ?? string.Empty;

                _usersPersonalDetailDataProvider.AddOrUpdateUsersPersonalDetail(_usersPersonalDetail);
            }

            if (_newPhotoDetails != null)
            {
                _photoDetailDataProvider.AddOrUpdatePhotoDetail(_newPhotoDetails);

                if (_isActivePhotoUpdate && _originalPhotoDetail != null)
                    _originalPhotoDetail.IsChecked = false;
            }

            if (_isActivePhotoUpdate && _originalPhotoDetail != null)
                _photoDetailDataProvider.AddOrUpdatePhotoDetail(_originalPhotoDetail);

            if(_allPhotoDetails != null && _allPhotoDetails.Any())
            {
                List<PhotoDetails> deletePhotoList = 
                    _allPhotoDetails.Where(remove => remove.IsPhotoRemoved).ToList();

                if(deletePhotoList != null && deletePhotoList.Any())
                {
                    foreach (PhotoDetails deletePhoto in deletePhotoList)
                        _photoDetailDataProvider.AddOrUpdatePhotoDetail(deletePhoto);
                }
            }

            if (_removePhoto != null && _removePhoto.Any())
            {
                foreach (PhotoDetails deletePhoto in _removePhoto)
                    _photoDetailDataProvider.DeleteDocumentFileFromFolderAsync(deletePhoto.LocalPhotoName);
            }

            LoginDetail loginDetail = new LoginDetail
            {
                UserId = _usersPersonalDetail.Mobile,
                EmailId = _usersPersonalDetail.EmailId
            }
            ;
            _navigationService.Navigate(typeof(WelcomePage), loginDetail);
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode navigationMode)
        {
            await base.OnNavigatedToAsync(parameter, navigationMode).ConfigureAwait(false);
            InitializeListData();

            if (parameter is UsersPersonalDetail usersPersonalDetail)
            {
                _usersPersonalDetail = usersPersonalDetail;

                if (_usersPersonalDetail != null)
                {
                    _originalPhotoDetail = _photoDetailDataProvider.GetActivePhotoDetail(
                        _usersPersonalDetail.EmailId);

                    _photoName = _originalPhotoDetail != null ?
                        _originalPhotoDetail.LocalPhotoName : string.Empty;

                    _fullName = _usersPersonalDetail.FullName;
                    _address = _usersPersonalDetail.Address;
                    SetCountry(_usersPersonalDetail.Country);
                    SetState(_usersPersonalDetail.State);
                    SetCity(_usersPersonalDetail.City);

                    await SetPhotoAsync(_photoName);
                }
            }
        }

        private void SetCity(string city)
        {
            if (!string.IsNullOrEmpty(city) && _cityList != null)
                _selectedCity = _cityList.FirstOrDefault(name => name.Name == city);
        }

        private void SetState(string state)
        {
            if (_stateList != null && !string.IsNullOrEmpty(state))
            {
                _selectedState = _stateList.FirstOrDefault(name => name.Name == state);

                if (_selectedState != null && _masterStateList != null)
                {
                    _isCityVisible = true;
                    List<City> _masterCityList = _masterStateList.FirstOrDefault(
                        id => id.Id == _selectedState.Id).Cities;

                    if (_masterCityList != null)
                        GenerateListForSingleSelection(_masterCityList);
                }
            }
        }

        private void SetCountry(string country)
        {
            if (_countryList != null && !string.IsNullOrEmpty(country))
            {
                _selectedCountry = _countryList.FirstOrDefault(name => name.Name == country);

                if (_selectedCountry != null && _masterCountryList != null)
                {
                    _isStateVisible = true;
                    _masterStateList = _masterCountryList.FirstOrDefault(
                        id => id.Id == _selectedCountry.Id)?.States;

                    if (_masterStateList != null)
                        GenerateListForSingleSelection(_masterStateList);
                }
            }
        }

        private async Task SetPhotoAsync(string photoName)
        {
            if (!string.IsNullOrEmpty(photoName))
            {
                StorageFolder storageFolder = await ApplicationData.Current.LocalFolder.
                    GetFolderAsync(ApplicationConstants.Document);

                if (await storageFolder.TryGetItemAsync(photoName) is StorageFile file)
                    _imageSource = await PhotoUtility.GetDecrtypedPhotoAsync(file, ApplicationConstants.Document);
                else
                    _imageSource = null;
            }
            else
            {
                _imageSource = null;
            }

            OnPropertyChanged(() => ImageSource);
        }

        private void InitializeListData()
        {
            _masterCountryList =
                new ReadOnlyCollection<Country>(_countryDataProvider.GetCountryData());

            if (_masterCountryList != null && _masterCountryList.Any())
                GenerateListForSingleSelection(_masterCountryList);
        }

        private void GenerateListForSingleSelection(IReadOnlyCollection<object> singleCode)
        {
            if (singleCode is IReadOnlyCollection<Country> countryList)
            {
                _countryList = countryList.
                    Select(code =>
                        new SingleSelection
                        {
                            Id = code.Id,
                            Name = code.Name,
                            JsonData = JsonConvert.SerializeObject(code.States)
                        }).ToList();
            }
            else if (singleCode is IReadOnlyCollection<State> stateList)
            {
                _stateList = stateList.
                    Select(code =>
                        new SingleSelection
                        {
                            Id = code.Id,
                            Name = code.Name,
                            JsonData = JsonConvert.SerializeObject(code.Cities)
                        }).ToList();
            }
            else if (singleCode is IReadOnlyCollection<City> cityList)
            {
                _cityList = cityList.
                    Select(code =>
                        new SingleSelection
                        {
                            Id = code.Id,
                            Name = code.Name
                        }).ToList();
            }
        }

        public UsersPersonalDetail UsersPersonalDetail
        {
            get => _usersPersonalDetail;
            set => _usersPersonalDetail = value;
        }

        public ICommand SaveUserDetailCommand => _saveUserDetailCommand;
        public ICommand ChangedCountryCommand => _changedCountryCommand;
        public ICommand ChangedStateCommand => _changedStateCommand;
        public ICommand ChangedCityCommand => _changedCityCommand;

        [NameValidationAttribute(ErrorMessage = ApplicationConstants.NameRequired)]
        public string FullName
        {
            get => _fullName;
            set => _fullName = value;
        }

        [Required(ErrorMessage = ApplicationConstants.AddressRequired)]
        [AddressValidationAttribute(ErrorMessage = ApplicationConstants.AddressLimitError)]
        public string Address
        {
            get => _address;
            set => _address = value;
        }

        public string PhotoName
        {
            get => _photoName;
            set => _photoName = value;
        }

        public BitmapImage ImageSource
        {
            get => _imageSource;
            set => _imageSource = value;
        }

        public SingleSelection SelectedCountry
        {
            get => _selectedCountry;
            set => _selectedCountry = value;
        }

        public SingleSelection SelectedState
        {
            get => _selectedState;
            set => _selectedState = value;
        }

        public SingleSelection SelectedCity
        {
            get => _selectedCity;
            set => _selectedCity = value;
        }

        public bool IsStateVisible
        {
            get => _isStateVisible;
            set => _isStateVisible = value;
        }

        public bool IsCityVisible
        {
            get => _isCityVisible;
            set => _isCityVisible = value;
        }

        public long MaxLength => ApplicationConstants.AddressLimit;

        public ICommand RetakePhotoCommand => _retakePhotoCommand;

        public ICommand PreviousPhotoCommand => _previousPhotoCommand;

        public ICommand RemovePhotoCommand => _removePhotoCommand;

        public bool IsPreviousButtonVisible
        {
            get => _isPreviousButtonVisible;
            set => _isPreviousButtonVisible = value;
        }

        public bool IsRemoveButtonVisible
        {
            get => _isRemoveButtonVisible;
            set => _isRemoveButtonVisible = value;
        }
    }
}
