using BookingBanquetDV.Converters;
using BookingBanquetDV.Utility;
using BookingBanquetDV.Validation;
using BookingServices.Interfaces;
using CommonServices.ConstantData;
using CommonServices.Extensions;
using CommonServices.Model;
using CommonServices.Model.City;
using CommonServices.Utilities;
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
        private Users _users;
        private bool _isResendOTP;
        private City _selectedCity;
        private State _selectedState;
        private Country _selectedCountry;
        private string _address;
        private IReadOnlyCollection<Country> _countryList;
        private IReadOnlyCollection<State> _stateList;
        private IReadOnlyCollection<City> _cityList;
        private bool _isCityVisible;
        private bool _isStateVisible;
        private string _selectedPhoneCode;
        private IReadOnlyCollection<string> _phoneCodeList;
        private string _photoName;
        private readonly ICommand _changedCountryCommand;
        private readonly ICommand _changedStateCommand;
        private readonly ICommand _resetCommand;
        private readonly ICommand _signupCommand;
        private readonly ICommand _oTPCommand;
        private readonly ICommand _loginCommand;
        private readonly IMessageDialogService _messageDialogService;
        private readonly INavigationService _navigationService;
        private readonly IOfflineDatabaseProvider _offlineDatabaseProvider;
        private readonly ICountryDataProvider _countryDataProvider;
        private ICameraViewModel _cameraViewModel;
        private bool _isEmailSent;

        public SignupViewModel(
            INavigationService navigationService,
            IOfflineDatabaseProvider offlineDatabaseProvider,
            IMessageDialogService messageDialogService,
            ICountryDataProvider countryDataProvider,
            ICameraViewModel cameraViewModel)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _offlineDatabaseProvider = offlineDatabaseProvider ?? throw new ArgumentNullException(nameof(offlineDatabaseProvider));
            _messageDialogService = messageDialogService ?? throw new ArgumentNullException(nameof(messageDialogService));
            _countryDataProvider = countryDataProvider ?? throw new ArgumentNullException(nameof(countryDataProvider));
            _cameraViewModel = cameraViewModel ?? throw new ArgumentNullException(nameof(cameraViewModel));

            _oTPCommand = new DelegateCommand(
                async parameter => await OnOTPClickAsync(parameter));

            _loginCommand = new DelegateCommand(
                parameter => OnLoginClick());

            _signupCommand = new DelegateCommand(
                async parameter => await OnSignUpClickAsync());

            _resetCommand = new DelegateCommand(
                parameter => OnResetClick());

            _changedCountryCommand = new DelegateCommand(
                parameter => OnCountryChanged(parameter));

            _changedStateCommand = new DelegateCommand(
                parameter => OnStateChanged(parameter));
        }

        private void OnStateChanged(object parameter)
        {
            if (_selectedState != null)
            {
                _isCityVisible = true;
                _cityList = new ReadOnlyCollection<City>(_selectedState.Cities);

                OnPropertyChanged(() => IsCityVisible);
                OnPropertyChanged(() => CityList);
            }
        }

        private void OnCountryChanged(object parameter)
        {
            if (_selectedCountry != null)
            {
                _isStateVisible = true;
                _stateList = new ReadOnlyCollection<State>(_selectedCountry.States);

                OnPropertyChanged(() => IsStateVisible);
                OnPropertyChanged(() => StateList);
            }
        }

        private void OnResetClick()
        {
            if (!_isEmailSent)
            {
                _fullName = _emailId = _mobileNo = _password =
                    _confirmPassword = Address = _selectedPhoneCode = string.Empty;
                _selectedCountry = null;
                _selectedState = null;
                _selectedCity = null;

                _isOtpEnable = false;

                OnPropertyChanged(() => IsOtpEnable);
                OnPropertyChanged(() => FullName);
                OnPropertyChanged(() => EmailId);
                OnPropertyChanged(() => MobileNo);
                OnPropertyChanged(() => Password);
                OnPropertyChanged(() => ConfirmPassword);
                OnPropertyChanged(() => SelectedPhoneCode);
                OnPropertyChanged(() => SelectedCountry);
                OnPropertyChanged(() => SelectedState);
                OnPropertyChanged(() => SelectedCity);
            }
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

                    if (photoDetails != null)
                        _users.PhotoName = photoDetails.LocalPhotoName;

                    App.ShowLoadingIndicator(ApplicationConstants.SendingEmail);
                    await SendBookingMailAsync();
                    App.HideLoadingIndicator();

                    _somethingWrongMessage = false;
                    bool isSaved = _offlineDatabaseProvider.AddOrUpdateUsers(_users);
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
            if (_users != null)
            {
                EmailParameter emailParameter = new EmailParameter
                {
                    EmailAddress = _users.EmailId,
                    UserName = _users.FullName,
                    Users = _users
                };

                return await EmailUtility.SendEmailsAsync(emailParameter);
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
                _users = new Users
                {
                    EmailId = _emailId,
                    FullName = _fullName,
                    Mobile = _mobileNo,
                    Password = _password,
                    UserId = _emailId,
                    CreationDate = DateTime.Now.ConvertToString(
                        ApplicationConstants.DatabaseDateTimeFormat),
                    IsActivate = false,
                    PasswordChanged = 0,
                    Address = _address,
                    City = _selectedCity.Name,
                    Country = _selectedCountry.Name,
                    State = _selectedState.Name,
                    PhoneCode = _selectedPhoneCode,
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
            return await _cameraViewModel.OnClickTakeAPhotoCommandAsync();
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
            _isEmailSent = await EmailUtility.SendEmailsAsync(emailParameter);
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
            _countryList = new ReadOnlyCollection<Country>(_countryDataProvider.GetCountryData());
            if(_countryList != null && _countryList.Any())
            {
                _phoneCodeList = _countryList.Select(phone => phone.PhoneCode).ToList();
                _phoneCodeList = _phoneCodeList.OrderBy(phone => phone).ToList();
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
        public Country SelectedCountry
        {
            get => _selectedCountry;
            set => _selectedCountry = value;
        }

        public State SelectedState
        {
            get => _selectedState;
            set => _selectedState = value;
        }

        public City SelectedCity
        {
            get => _selectedCity;
            set => _selectedCity = value;
        }

        [Required(ErrorMessage = ApplicationConstants.PhoneCodeRequired)]
        public string SelectedPhoneCode
        {
            get => _selectedPhoneCode;
            set => _selectedPhoneCode = value;
        }

        public IReadOnlyCollection<Country> CountryList
        {
            get => _countryList;
            set => _countryList = value;
        }

        public IReadOnlyCollection<State> StateList
        {
            get => _stateList;
            set => _stateList = value;
        }

        public IReadOnlyCollection<City> CityList
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

        public IReadOnlyCollection<string> PhoneCodeList
        {
            get => _phoneCodeList;
            set => _phoneCodeList = value;
        }
    }
}
