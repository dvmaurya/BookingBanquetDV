using BookingBanquetDV.Converters;
using BookingBanquetDV.Utility;
using BookingBanquetDV.Validation;
using BookingServices.Interfaces;
using CommonServices.Analytcis;
using CommonServices.ConstantData;
using CommonServices.Extensions;
using CommonServices.Model;
using CommonServices.Model.City;
using CommonServices.Utilities;
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
using Windows.UI.Xaml.Navigation;

namespace BookingBanquetDV.Source.Register
{
    public class SignupViewModel : ValidationViewModelBase, ISignupViewModel, INotesValidator
    {
        private string _fullName;
        private string _emailId;
        private string _mobileNo;
        private string _password;
        private string _confirmPassword;
        private bool _isEmailExist;
        private bool _isMobileExist;
        private bool _somethingWrongMessage;
        private bool _isOtpEnable;
        private string _oTP;
        private string _generatedOTP;
        private UsersCred _usersCred;
        private UsersPersonalDetail _usersPersonalDetail;
        private bool _isResendOTP;
        private SingleSelection _selectedCity;
        private SingleSelection _selectedState;
        private SingleSelection _selectedCountry;
        private string _address;
        private IReadOnlyCollection<SingleSelection> _countryList;
        private IReadOnlyCollection<SingleSelection> _stateList;
        private IReadOnlyCollection<SingleSelection> _cityList;
        private bool _isCityVisible;
        private bool _isStateVisible;
        private readonly ICommand _changedCountryCommand;
        private readonly ICommand _changedStateCommand;
        private readonly ICommand _changedCityCommand;
        private readonly ICommand _resetCommand;
        private readonly ICommand _signupCommand;
        private readonly ICommand _oTPCommand;
        private readonly ICommand _loginCommand;
        private readonly IMessageDialogService _messageDialogService;
        private readonly INavigationService _navigationService;
        private readonly IOfflineDatabaseProvider _offlineDatabaseProvider;
        private readonly ICountryDataProvider _countryDataProvider;
        private readonly IUsersPersonalDetailDataProvider _userPersonalDetailDataProvider;
        private readonly IPhotoDetailDataProvider _photoDetailDataProvider;
        private readonly IAnalyticsManager _analyticsManager;

        private ICameraViewModel _cameraViewModel;
        private bool _isEmailSent;
        private PhotoDetails _photoDetail;

        public SignupViewModel(
            INavigationService navigationService,
            IOfflineDatabaseProvider offlineDatabaseProvider,
            IMessageDialogService messageDialogService,
            ICountryDataProvider countryDataProvider,
            ICameraViewModel cameraViewModel,
            IUsersPersonalDetailDataProvider usersPersonalDetailDataProvider,
            IPhotoDetailDataProvider photoDetailDataProvider,
            IAnalyticsManager analyticsManager)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");
            _offlineDatabaseProvider = offlineDatabaseProvider ?? throw new ArgumentNullException("offlineDatabaseProvider");
            _messageDialogService = messageDialogService ?? throw new ArgumentNullException("messageDialogService");
            _countryDataProvider = countryDataProvider ?? throw new ArgumentNullException("countryDataProvider");
            _cameraViewModel = cameraViewModel ?? throw new ArgumentNullException("cameraViewModel");
            _userPersonalDetailDataProvider = usersPersonalDetailDataProvider ?? throw new ArgumentNullException("usersPersonalDetailDataProvider");
            _photoDetailDataProvider = photoDetailDataProvider ?? throw new ArgumentNullException("photoDetailDataProvider");
            _analyticsManager = analyticsManager ?? throw new ArgumentNullException("analyticsManager");

            _oTPCommand = new DelegateCommand(
                async parameter => await OnOTPClickAsync(parameter));

            _loginCommand = new DelegateCommand(
                parameter => OnLoginClick());

            _signupCommand = new DelegateCommand(
                async parameter => await OnSignUpClickAsync());

            _resetCommand = new DelegateCommand(
                parameter => OnResetClick());

            _changedCountryCommand = new DelegateCommand(
                async parameter => await OnCountryChangedAsync());

            _changedStateCommand = new DelegateCommand(
                async parameter => await OnStateChangedAsync());

            _changedCityCommand = new DelegateCommand(
                async parameter => await OnCityChangedAsync());
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

        private void OnResetClick()
        {
            _fullName = _emailId = _mobileNo = _password =
                    _confirmPassword = Address = string.Empty;
            _selectedCountry = null;
            _selectedState = null;
            _selectedCity = null;
            _isStateVisible = _isCityVisible = false;

            _isOtpEnable = false;

            OnPropertyChanged(() => IsOtpEnable);
            OnPropertyChanged(() => FullName);
            OnPropertyChanged(() => EmailId);
            OnPropertyChanged(() => MobileNo);
            OnPropertyChanged(() => Password);
            OnPropertyChanged(() => ConfirmPassword);
            OnPropertyChanged(() => SelectedCountry);
            OnPropertyChanged(() => SelectedState);
            OnPropertyChanged(() => SelectedCity);
            OnPropertyChanged(() => IsStateVisible);
            OnPropertyChanged(() => IsCityVisible);
        }

        private async Task OnSignUpClickAsync()
        {
            if (ValidateProperties())
            {
                if (_oTP.FormatStringEquals(_generatedOTP))
                {
                    await _messageDialogService.ShowMessageDialogAsync(
                        ApplicationConstants.TakeAPhoto,
                        ApplicationConstants.GoForPhoto);

                    PhotoDetails photoDetails = await OnCameraClickAsync();

                    if (photoDetails != null && _usersPersonalDetail != null)
                    {
                        _photoDetail = new PhotoDetails
                        {
                            LocalPhotoName = photoDetails.LocalPhotoName,
                            Date = photoDetails.Date,
                            FileType = photoDetails.FileType,
                            Size = photoDetails.Size,
                            EmailId = _emailId,
                            IsChecked = true,
                            IsPhotoRemoved = false
                        };
                    }

                    bool isSaved = _offlineDatabaseProvider.AddOrUpdateUsers(_usersCred);
                    if (isSaved)
                    {
                        _userPersonalDetailDataProvider.AddOrUpdateUsersPersonalDetail(_usersPersonalDetail);
                        _photoDetailDataProvider.AddOrUpdatePhotoDetail(_photoDetail);
                    }

                    await _messageDialogService.ShowMessageDialogAsync(
                        ApplicationConstants.Success,
                        ApplicationConstants.SignupSuccess);

                    //GoogleAPI.SendToGoogleAPIAsync(_users);

                    _navigationService.Navigate(typeof(MainPage));
                }
                else
                {
                    _somethingWrongMessage = true;
                }

                OnPropertyChanged(() => SomethingWrongMessage);
            }
        }

        private async Task<bool> SendBookingMailAsync()
        {
            if (_usersCred != null)
            {
                EmailParameter emailParameter = new EmailParameter
                {
                    EmailAddress = _usersCred.EmailId,
                    UserName = _usersPersonalDetail.FullName,
                    UsersPersonalDetail = _usersPersonalDetail,
                    PhotoDetails = _photoDetail
                };

                return await EmailUtility.SendEmailsAsync(emailParameter, _analyticsManager);
            }

            return false;
        }

        private void OnLoginClick()
        {
            _navigationService.Navigate(typeof(MainPage));
        }

        private async Task OnOTPClickAsync(object parameter)
        {
            CheckUserExist();
            if (parameter is string resendOtp && resendOtp.FormatStringEquals("ResendOtp"))
                _isResendOTP = true;

            if (ValidateProperties())
            {
                _usersCred = new UsersCred
                {
                    EmailId = _emailId,
                    Mobile = _mobileNo,
                    Password = _password,
                    UserId = _emailId,
                    CreationDate = DateTime.Now.ConvertToString(
                        ApplicationConstants.DatabaseDateTimeFormat),
                    IsAdmin = false,
                    PasswordChanged = 0
                };

                _usersPersonalDetail = new UsersPersonalDetail
                {
                    Address = _address,
                    Country = _selectedCountry?.Name,
                    State = _selectedState?.Name ?? "NA",
                    City = _selectedCity?.Name ?? "NA",
                    EmailId = _emailId,
                    FullName = _fullName,
                    Mobile = _mobileNo,
                    CreationDate = DateTime.Now.ConvertToString(
                        ApplicationConstants.DatabaseDateTimeFormat),
                };

                await SendOtpAsync();
            }

            _isMobileExist = false;
            _isEmailExist = false;

            OnPropertyChanged(() => IsOtpEnable);
            OnPropertyChanged(() => IsMobileExist);
            OnPropertyChanged(() => IsEmailExist);
        }

        private async Task<PhotoDetails> OnCameraClickAsync()
        {
            return await _cameraViewModel.OnClickTakeAPhotoAsync(ApplicationConstants.Document);
        }

        private void CheckUserExist()
        {
            if (!string.IsNullOrEmpty(_emailId) && !string.IsNullOrEmpty(_mobileNo))
            {
                List<string> existUsers = _offlineDatabaseProvider.GetAllUsers();

                if (existUsers != null && existUsers.Any())
                {
                    foreach (string existUser in existUsers)
                    {
                        if (existUser.FormatStringEquals(_emailId))
                        {
                            _isEmailExist = true;
                            break;
                        }
                        else if (existUser.FormatStringEquals(_mobileNo))
                        {
                            _isMobileExist = true;
                            break;
                        }
                    }
                }
            }
        }

        private async Task SendOtpAsync()
        {
            _generatedOTP = Utilities.GenerateOTP();

            EmailParameter emailParameter = new EmailParameter
            {
                EmailAddress = _emailId,
                IsRecovery = false,
                IsSignup = true,
                Otp = _generatedOTP,
                UserName = _fullName
            };

            App.ShowLoadingIndicator(ApplicationConstants.SendingEmail);
            _isEmailSent = await EmailUtility.SendEmailsAsync(emailParameter, _analyticsManager);
            App.HideLoadingIndicator();

            if (_isEmailSent)
            {
                _isOtpEnable = true;
                _somethingWrongMessage = false;
            }
            else
            {
                await _messageDialogService.ShowMessageDialogAsync(
                    ApplicationConstants.Warning,
                    ApplicationConstants.LowInternet);

                _somethingWrongMessage = true;
                _isOtpEnable = false;
            }
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode navigationMode)
        {
            await base.OnNavigatedToAsync(parameter, navigationMode).ConfigureAwait(false);
            InitializeListData();
        }

        private void InitializeListData()
        {
            ReadOnlyCollection<Country> countryList =
                new ReadOnlyCollection<Country>(_countryDataProvider.GetCountryData());

            if (countryList != null && countryList.Any())
                GenerateListForSingleSelection(countryList);
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

        [NameValidationAttribute(ErrorMessage = ApplicationConstants.NameRequired)]
        public string FullName
        {
            get => _fullName;
            set => _fullName = value;
        }

        [EmailValidationAttribute(ErrorMessage = ApplicationConstants.InvalidEmail)]
        public string EmailId
        {
            get => _emailId;
            set => _emailId = value;
        }

        [MobileNoValidationAttribute(ErrorMessage = ApplicationConstants.InvalidMobileNo)]
        public string MobileNo
        {
            get => _mobileNo;
            set => _mobileNo = value;
        }

        [PasswordValidationAttribute(ErrorMessage = ApplicationConstants.PasswordLength)]
        public string Password
        {
            get => _password;
            set => _password = value;
        }

        [PasswordMatchValidationAttribute(ErrorMessage = ApplicationConstants.PasswordMatch)]
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => _confirmPassword = value;
        }

        public bool IsEmailExist
        {
            get => _isEmailExist;
            set => _isEmailExist = value;
        }

        public bool IsMobileExist
        {
            get => _isMobileExist;
            set => _isMobileExist = value;
        }

        [OTPValidationAttribute(ErrorMessage = ApplicationConstants.InvalidOTP)]
        public string OTP
        {
            get => _oTP;
            set => _oTP = value;
        }

        public string GeneratedOTP
        {
            get => _generatedOTP;
            set => _generatedOTP = value;
        }

        public ICommand OTPCommand => _oTPCommand;
        public ICommand LoginCommand => _loginCommand;
        public ICommand SignupCommand => _signupCommand;
        public ICommand ResetCommand => _resetCommand;

        public bool IsOtpEnable
        {
            get => _isOtpEnable;
            set => _isOtpEnable = value;
        }

        public bool SomethingWrongMessage
        {
            get => _somethingWrongMessage;
            set => _somethingWrongMessage = value;
        }

        public bool IsResendOTP
        {
            get => _isResendOTP;
            set => _isResendOTP = value;
        }

        public long MaxLength => ApplicationConstants.AddressLimit;

        [Required(ErrorMessage = ApplicationConstants.AddressRequired)]
        [AddressValidationAttribute(ErrorMessage = ApplicationConstants.AddressLimitError)]
        public string Address
        {
            get => _address;
            set => _address = value;
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

        public IReadOnlyCollection<SingleSelection> CountryList
        {
            get => _countryList;
            set => _countryList = value;
        }

        public IReadOnlyCollection<SingleSelection> StateList
        {
            get => _stateList;
            set => _stateList = value;
        }

        public IReadOnlyCollection<SingleSelection> CityList
        {
            get => _cityList;
            set => _cityList = value;
        }

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

        public ICommand ChangedCountryCommand => _changedCountryCommand;

        public ICommand ChangedStateCommand => _changedStateCommand;

        public ICommand ChangedCityCommand => _changedCityCommand;
    }
}
