using BookingBanquetDV.Converters;
using BookingBanquetDV.Source.Camera;
using BookingBanquetDV.Source.Register;
using BookingBanquetDV.Utility;
using BookingBanquetDV.Validation;
using BookingServices.Interfaces;
using CommonServices.Analytcis;
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
using Windows.UI.Xaml.Navigation;

namespace BookingBanquetDV.Source.Welcome
{
    public class NewBookingViewModel : ValidationViewModelBase, INewBookingViewModel, INotesValidator
    {
        private string _customerName;
        private string _bookedBy;
        private string _totalRooms;
        private string _totalFlours;
        private string _amount;
        private string _advanced;
        private string _balance;

        private List<string> _confirmList;
        private DateTimeOffset _maxDate;
        private DateTimeOffset _minDateTime;
        private DateTimeOffset _bookingDate;
        private BookingDetails _bookingDetails;
        private LoginDetail _loginDetail;
        private string _selectedConfirmation;
        private readonly ICommand _saveCommand;
        private DateTimeOffset _creationDate;
        private string _mobileNo;
        private string _emailId;
        private IReadOnlyCollection<SingleSelection> _countryList;
        private IReadOnlyCollection<SingleSelection> _cityList;
        private IReadOnlyCollection<SingleSelection> _stateList;
        private bool _isCityVisible;
        private bool _isStateVisible;
        private readonly ICommand _changedCountryCommand;
        private readonly ICommand _changedStateCommand;
        private readonly ICommand _changedCityCommand;
        private readonly ICommand _takePhotoCommand;
        private readonly ICommand _removePhotoCommand;
        private IReadOnlyCollection<Country> _masterCountryList;
        private IReadOnlyCollection<State> _masterStateList;
        private SingleSelection _selectedCity;
        private SingleSelection _selectedState;
        private SingleSelection _selectedCountry;
        private CustomerPhotoDetail _customerPhoto;
        private BanquetDetail _banquetDetail;
        private IList<CustomerPhotoDetail> _deletePhoto;
        private IReadOnlyCollection<BookingDetails> _masterBookingDetailList;
        private string _address;
        private IList<string> _totalFLourList;
        private IList<string> _totalRoomList;
        private readonly ICustomerPhotoDataProvider _customerPhotoDataProvider;
        private readonly IMessageDialogService _messageDialogService;
        private readonly IBookingDataProvider _bookingDataProvider;
        private readonly ICountryDataProvider _countryDataProvider;
        private readonly ICameraViewModel _cameraViewModel;
        private readonly INavigationService _navigationService;
        private readonly IBanquetDataProvider _banquetDataProvider;
        private readonly IAnalyticsManager _analyticsManager;
        
        public NewBookingViewModel(
            ICountryDataProvider countryDataProvider,
            ICameraViewModel cameraViewModel,
            IBookingDataProvider bookingDataProvider,
            IMessageDialogService messageDialogService,
            INavigationService navigationService,
            ICustomerPhotoDataProvider customerPhotoDataProvider,
            IBanquetDataProvider banquetDataProvider,
            IAnalyticsManager analyticsManager)
        {
            _countryDataProvider = countryDataProvider ?? throw new ArgumentNullException("countryDataProvider");
            _cameraViewModel = cameraViewModel ?? throw new ArgumentNullException("cameraViewModel");
            _bookingDataProvider = bookingDataProvider ?? throw new ArgumentNullException("bookingDataProvider");
            _messageDialogService = messageDialogService ?? throw new ArgumentNullException("messageDialogService");
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");
            _customerPhotoDataProvider = customerPhotoDataProvider ?? throw new ArgumentNullException("customerPhotoDataProvider");
            _banquetDataProvider = banquetDataProvider ?? throw new ArgumentNullException("banquetDataProvider");
            _analyticsManager = analyticsManager ?? throw new ArgumentNullException("analyticsManager");

            _deletePhoto = new List<CustomerPhotoDetail>();
            _totalFLourList = new ObservableCollection<string>();
            _totalRoomList = new ObservableCollection<string>();

            _saveCommand = new DelegateCommand(async parameter => await OnSaveNewBookingAsync());

            _changedCountryCommand = new DelegateCommand(
               async parameter => await OnCountryChangedAsync());

            _changedStateCommand = new DelegateCommand(
                async parameter => await OnStateChangedAsync());

            _changedCityCommand = new DelegateCommand(
                async parameter => await OnCityChangedAsync());

            _takePhotoCommand = new DelegateCommand(
               async parameter => await OnTakePhotoAsync());

            _removePhotoCommand = new DelegateCommand(
                async parameter => await OnRemovePhotoAsync());
        }

        private async Task OnRemovePhotoAsync()
        {
            if (_customerPhoto != null)
            {
                List<IUICommand> commands = new List<IUICommand>
                    {
                        new UICommand(
                            label: ApplicationConstants.Yes,
                            action: (uiCommand) => OnContinueRemovePhoto()),

                        new UICommand(
                            label: ApplicationConstants.No,
                            action: null)
                    };

                await _messageDialogService.ShowMessageDialogAsync(
                    ApplicationConstants.Warning,
                    ApplicationConstants.RemovePhotoWarning,
                    commands);
            }
        }

        private void OnContinueRemovePhoto()
        {
            _deletePhoto.Add(_customerPhoto);
            _customerPhoto = null;

            OnPropertyChanged(() => SelectedPhotoDetails);
        }

        private async Task OnTakePhotoAsync()
        {
            CustomerPhotoDetail photoDetails = await _cameraViewModel.OnClickCustomerPhotoAsync(
                ApplicationConstants.CustomerPhoto);

            if (_customerPhoto != null)
            {
                _deletePhoto.Add(_customerPhoto);
                photoDetails.Id = _customerPhoto.Id;
            }

            _customerPhoto = photoDetails;

            if (_customerPhoto != null)
                await SetPhotoAsync();
        }

        private async Task SetPhotoAsync()
        {
            if (_customerPhoto != null &&
                !string.IsNullOrEmpty(_customerPhoto.PhotoName))
            {
                StorageFolder storageFolder = await ApplicationData.Current.LocalFolder.
                    GetFolderAsync(ApplicationConstants.CustomerPhoto);

                if (await storageFolder.TryGetItemAsync(_customerPhoto.PhotoName) is StorageFile file)
                    _customerPhoto.ImageSource = await PhotoUtility.GetDecrtypedPhotoAsync(file, ApplicationConstants.CustomerPhoto);

                OnPropertyChanged(() => SelectedPhotoDetails);
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
                IReadOnlyCollection<City> cityList =
                    JsonConvert.DeserializeObject<IReadOnlyCollection<City>>(_selectedState.JsonData);

                if (cityList != null)
                {
                    _isCityVisible = true;
                    GenerateListForSingleSelection(cityList);
                }

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
                IReadOnlyCollection<State> stateList =
                    JsonConvert.DeserializeObject<IReadOnlyCollection<State>>(_selectedCountry.JsonData);

                if (stateList != null)
                {
                    _isStateVisible = true;
                    GenerateListForSingleSelection(stateList);
                }

                await OnStateChangedAsync();
            }

            OnPropertyChanged(() => IsStateVisible);
            OnPropertyChanged(() => SelectedCountry);
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode navigationMode)
        {
            await base.OnNavigatedToAsync(parameter, navigationMode).ConfigureAwait(false);

            GetConfirmationList();
            InitializeListData();

            if (parameter is BookingParameter bookingParameter)
            {
                _bookingDetails = bookingParameter.BookingDetails;
                _loginDetail = bookingParameter.LoginDetail;

                _bookedBy = _loginDetail?.UserName;

                await OnPageLoadingAsync();
                LoadAllBookingDetail();
            }
        }

        private async Task OnPageLoadingAsync()
        {
            _maxDate = DateTimeOffset.Now.Date;
            _minDateTime = DateTimeOffset.Now.Date;
            _bookingDate = DateTimeOffset.Now.Date;

            if (_bookingDetails != null)
            {
                SetBookingDetail(_bookingDetails);
                _customerPhoto = _customerPhotoDataProvider.
                    GetCustomerPhotoDetail(_bookingDetails.Id);

                await SetPhotoAsync();
            }
            else
            {
                _creationDate = DateTime.Now.Date;
            }
        }

        private void LoadAllBookingDetail()
        {
            _masterBookingDetailList = new ObservableCollection<BookingDetails>(
                _bookingDataProvider.GetAllBookings());
        }

        private void InitializeListData()
        {
            _masterCountryList =
                new ReadOnlyCollection<Country>(_countryDataProvider.GetCountryData());

            if (_masterCountryList != null && _masterCountryList.Any())
                GenerateListForSingleSelection(_masterCountryList);

            _banquetDetail = _banquetDataProvider.GetBanquetDetail();

            if (_banquetDetail != null)
            {
                if(!string.IsNullOrEmpty(_banquetDetail.AvailableFlours) && 
                    _banquetDetail.AvailableFlours.ToInt32() != 0)
                {
                    for (int i = 0; i < _banquetDetail.AvailableFlours.ToInt32(); i++)
                        _totalFLourList.Add((i + 1).ConvertToString());
                }

                if (!string.IsNullOrEmpty(_banquetDetail.AvailableRooms) && 
                    _banquetDetail.AvailableRooms.ToInt32() != 0)
                {
                    for (int i = 0; i < _banquetDetail.AvailableRooms.ToInt32(); i++)
                        _totalRoomList.Add((i + 1).ConvertToString());
                }
            }
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

        private void SetBookingDetail(BookingDetails bookingDetails)
        {
            if (_bookingDetails != null)
            {
                _customerName = _bookingDetails.CustomerName;
                _mobileNo = _bookingDetails.MobileNo;
                _emailId = _bookingDetails.EmailId;
                _bookingDate = _bookingDetails.BookingDate != null ?
                    _bookingDetails.BookingDate.ConvertToDateTime() :
                    DateTimeOffset.Now.Date;
                _creationDate = _bookingDetails.CreationDate.ConvertToDateTime();
                _totalRooms = _bookingDetails.TotalRooms;
                _totalFlours = _bookingDetails.TotalFlours;
                _amount = _bookingDetails.TotalAmount;
                _advanced = _bookingDetails.Advanced;
                _balance = _bookingDetails.Balance;
                _selectedConfirmation = _bookingDetails.Confirmation;
                _address = _bookingDetails.Address;
                SetCountry(_bookingDetails.Country);
                SetState(_bookingDetails.State);
                SetCity(_bookingDetails.City);

                OnPropertyChanged(() => CustomerName);
                OnPropertyChanged(() => MobileNo);
                OnPropertyChanged(() => EmailId);
                OnPropertyChanged(() => BookingDate);
                OnPropertyChanged(() => TotalRooms);
                OnPropertyChanged(() => TotalFlours);
                OnPropertyChanged(() => TotalAmount);
                OnPropertyChanged(() => Balance);
                OnPropertyChanged(() => SelectedConfirmation);
                OnPropertyChanged(() => Address);
                OnPropertyChanged(() => SelectedCountry);
                OnPropertyChanged(() => SelectedState);
                OnPropertyChanged(() => SelectedCity);
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

        public async Task OnSaveNewBookingAsync()
        {
            if (ValidateProperties())
            {
                if (_customerPhoto == null)
                {
                    List<IUICommand> commands = new List<IUICommand>
                    {
                        new UICommand(
                            label: ApplicationConstants.Yes,
                            action: async(uiCommand) => await OnContinueSaveAsync()),

                        new UICommand(
                            label: ApplicationConstants.No,
                            action: null)
                    };

                    await _messageDialogService.ShowMessageDialogAsync(
                        ApplicationConstants.Warning,
                        ApplicationConstants.PhotoNotAvailable.FormatString(string.Empty),
                        commands);
                }
                else
                {
                    await OnContinueSaveAsync();
                }
            }
        }

        private async Task OnContinueSaveAsync()
        {
            if (_bookingDetails == null)
                _bookingDetails = new BookingDetails();

            _bookingDetails.Advanced = _advanced;
            _bookingDetails.Balance = _balance;
            _bookingDetails.BookedBy = _bookedBy;
            _bookingDetails.UserId = _loginDetail?.UserId;
            _bookingDetails.BookingDate = _bookingDate.ConvertToString(ApplicationConstants.DatabaseDateFormat);
            _bookingDetails.Confirmation = _selectedConfirmation ?? "Not decided";
            _bookingDetails.CreationDate = _creationDate.ConvertToString(ApplicationConstants.DatabaseDateFormat);
            _bookingDetails.CustomerName = _customerName;
            _bookingDetails.EmailId = _emailId;
            _bookingDetails.MobileNo = _mobileNo;
            _bookingDetails.TotalAmount = _amount;
            _bookingDetails.TotalFlours = _totalFlours ?? "NA";
            _bookingDetails.TotalRooms = _totalRooms ?? "NA";
            _bookingDetails.Address = _address;
            _bookingDetails.Country = _selectedCountry.Name;
            _bookingDetails.State = _selectedState?.Name ?? "NA";
            _bookingDetails.City = _selectedCity?.Name ?? "NA";

            await OnContinueBookingSaveAsync();

            await _messageDialogService.ShowMessageDialogAsync(
                ApplicationConstants.Congrats,
                ApplicationConstants.BookingSuccess.FormatString(
                    _bookingDetails.BookingDate,
                    _bookingDetails.CustomerName));

            _navigationService.Navigate(typeof(WelcomePage), _loginDetail);
        }

        private async Task OnContinueBookingSaveAsync()
        {
            if (_bookingDetails != null)
            {
                //bool isEmailSent = await SendingEmailAsync(_bookingDetails, _customerPhoto);
                //_bookingDetails.IsEmailSent = isEmailSent;
                _bookingDataProvider.AddOrUpdateBooking(_bookingDetails);

                if (_customerPhoto != null)
                {
                    _customerPhoto.BookingId = _bookingDetails.Id != 0 ?
                        _bookingDetails.Id : _masterBookingDetailList != null ?
                        _masterBookingDetailList.Count + 1 : 1;

                    _customerPhoto.EmailId = _bookingDetails.EmailId;
                    _customerPhotoDataProvider.AddOrUpdatePhotoDetail(_customerPhoto);
                }

                if (_deletePhoto != null && _deletePhoto.Any())
                {
                    foreach (CustomerPhotoDetail deletePhoto in _deletePhoto)
                        await _customerPhotoDataProvider.DeleteCustomerPhotoAsync(deletePhoto);
                }
            }
        }

        private async Task<bool> SendingEmailAsync(BookingDetails bookingDetail, CustomerPhotoDetail customerPhoto)
        {
            bool isEmailSent = false;
            App.ShowLoadingIndicator(ApplicationConstants.SendingEmail);
            isEmailSent = await SendBookingMailAsync(bookingDetail, customerPhoto);
            App.HideLoadingIndicator();

            return isEmailSent;
        }

        private async Task<bool> SendBookingMailAsync(BookingDetails bookingDetail, CustomerPhotoDetail customerPhoto)
        {
            if (bookingDetail != null)
            {
                EmailParameter emailParameter = new EmailParameter
                {
                    BookingDetails = bookingDetail,
                    EmailAddress = bookingDetail.EmailId,
                    UserName = bookingDetail.CustomerName,
                    CustomerPhoto = customerPhoto
                };

                return await EmailUtility.SendEmailsAsync(emailParameter, _analyticsManager);
            }

            return false;
        }

        private void GetConfirmationList()
        {
            _confirmList = new List<string>
            {
                "Yes",
                "No",
                "Not Decided"
            };
        }

        [Required(ErrorMessage = ApplicationConstants.NameRequired)]
        public string CustomerName
        {
            get => _customerName;
            set => _customerName = value;
        }

        [DateRequiredAttribute(ErrorMessage = ApplicationConstants.DateRequired)]
        public DateTimeOffset BookingDate
        {
            get { return _bookingDate; }
            set { SetValue(() => BookingDate, ref _bookingDate, value); }
        }

        public DateTimeOffset CreationDate => _creationDate;

        public DateTimeOffset MaxDate
        {
            get { return _maxDate; }
            set { SetValue(() => MaxDate, ref _maxDate, value); }
        }

        public DateTimeOffset MinDateTime
        {
            get { return _minDateTime; }
            set { SetValue(() => MinDateTime, ref _minDateTime, value); }
        }

        public string BookedBy
        {
            get => _bookedBy;
            set => _bookedBy = value;
        }

        public string TotalRooms
        {
            get => _totalRooms;
            set => _totalRooms = value;
        }

        public string TotalFlours
        {
            get => _totalFlours;
            set => _totalFlours = value;
        }

        [Required(ErrorMessage = ApplicationConstants.TotalAmountError)]
        public string TotalAmount
        {
            get => _amount;
            set => _amount = value;
        }

        [Required(ErrorMessage = ApplicationConstants.AdvanceAmountError)]
        public string Advanced
        {
            get => _advanced;
            set => _advanced = value;
        }

        public string Balance
        {
            get => _balance;
            set => _balance = value;
        }

        public List<string> ConfirmList => _confirmList;

        public string SelectedConfirmation
        {
            get => _selectedConfirmation;
            set => _selectedConfirmation = value;
        }

        [MobileNoValidationAttribute(ErrorMessage = ApplicationConstants.InvalidMobileNo)]
        public string MobileNo
        {
            get => _mobileNo;
            set => _mobileNo = value;
        }

        [EmailValidationAttribute(ErrorMessage = ApplicationConstants.InvalidEmail)]
        public string EmailId
        {
            get => _emailId;
            set => _emailId = value;
        }

        public ICommand SaveCommand => _saveCommand;

        public bool IsEmailSent { get; }

        public bool IsCityVisible
        {
            get => _isCityVisible;
            set => _isCityVisible = value;
        }

        public bool IsStateVisible
        {
            get => _isStateVisible;
            set => _isStateVisible = value;
        }

        [Required(ErrorMessage = ApplicationConstants.CountryRequired)]
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

        public ICommand ChangedCountryCommand => _changedCountryCommand;

        public ICommand ChangedStateCommand => _changedStateCommand;

        public ICommand ChangedCityCommand => _changedCityCommand;

        public long MaxLength => ApplicationConstants.AddressLimit;

        public ICommand TakePhotoCommand => _takePhotoCommand;

        public ICommand RemovePhotoCommand => _removePhotoCommand;

        public BookingDetails BookingDetails
        {
            get => _bookingDetails;
            set => _bookingDetails = value;
        }

        [Required(ErrorMessage = ApplicationConstants.AddressRequired)]
        [AddressValidationAttribute(ErrorMessage = ApplicationConstants.AddressLimitError)]
        public string Address
        {
            get => _address;
            set => _address = value;
        }

        public IReadOnlyCollection<BookingDetails> MasterBookingDetailList
        {
            get => _masterBookingDetailList;
            set => _masterBookingDetailList = value;
        }

        public CustomerPhotoDetail SelectedPhotoDetails
        {
            get => _customerPhoto;
            set => _customerPhoto = value;
        }

        public IList<string> TotalFLourList => _totalFLourList;

        public IList<string> TotalRoomList => _totalRoomList;
    }
}
