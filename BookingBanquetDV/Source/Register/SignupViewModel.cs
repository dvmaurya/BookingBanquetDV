using BookingBanquetDV.Utility;
using BookingBanquetDV.Validation;
using BookingServices.Interfaces;
using CommonServices.ConstantData;
using CommonServices.Extensions;
using CommonServices.Model;
using CommonServices.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewModelsAndValidation;
using ViewModelsAndValidation.Command;
using ViewModelsAndValidation.Services;
using ViewModelsAndValidation.ViewModels;
using Windows.ApplicationModel.Contacts;
using Windows.UI.Xaml.Navigation;

namespace BookingBanquetDV.Source.Register
{
    public class SignupViewModel : ValidationViewModelBase, ISignupViewModel
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
        private readonly ICommand _resetCommand;
        private readonly ICommand _signupCommand;
        private readonly ICommand _oTPCommand;
        private readonly ICommand _loginCommand;
        private readonly IMessageDialogService _messageDialogService;
        private readonly INavigationService _navigationService;
        private readonly IOfflineDatabaseProvider _offlineDatabaseProvider;

        public SignupViewModel(
            INavigationService navigationService,
            IOfflineDatabaseProvider offlineDatabaseProvider,
            IMessageDialogService messageDialogService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _offlineDatabaseProvider = offlineDatabaseProvider ?? throw new ArgumentNullException(nameof(offlineDatabaseProvider));
            _messageDialogService = messageDialogService ?? throw new ArgumentNullException(nameof(messageDialogService));

            _oTPCommand = new DelegateCommand(
                async parameter => await OnOTPClickAsync());

            _loginCommand = new DelegateCommand(
                parameter => OnLoginClick());

            _signupCommand = new DelegateCommand(
                async parameter => await OnSignUpClickAsync());

            _resetCommand = new DelegateCommand(
                parameter => OnResetClick());
        }

        private void OnResetClick()
        {
            if (_isOtpEnable)
            {
                _isOtpEnable = false;

                OnPropertyChanged(() => IsOtpEnable);
            }
        }

        private async Task OnSignUpClickAsync()
        {
            if (ValidateProperties())
            {
                if (_oTP.FormatStringEquals(_generatedOTP))
                {
                    _somethingWrongMessage = false;
                    bool isSaved = _offlineDatabaseProvider.AddOrUpdateUsers(_users);
                    await _messageDialogService.ShowMessageDialogAsync(
                        ApplicationConstants.Success,
                        ApplicationConstants.SignupSuccess).ConfigureAwait(true);

                    GoogleAPI.SendToGoogleAPIAsync(_users);

                    _navigationService.Navigate(typeof(MainPage));
                }
                else
                {
                    _somethingWrongMessage = true;
                }

                OnPropertyChanged(() => SomethingWrongMessage);
            }
        }

        private void OnLoginClick()
        {
            _navigationService.Navigate(typeof(MainPage));
        }

        private async Task OnOTPClickAsync()
        {
            CheckUserExist();

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
                    PasswordChanged = 0
                };

                await SendOtpAsync().ConfigureAwait(true);
            }

            _isMobileExist = false;
            _isEmailExist = false;

            OnPropertyChanged(() => IsOtpEnable);
            OnPropertyChanged(() => IsMobileExist);
            OnPropertyChanged(() => IsEmailExist);
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

            ContactEmail contactEmail = new ContactEmail
            {
                Address = _emailId,
                Description = _generatedOTP,
                Kind = ContactEmailKind.Work
            };

            bool isEmailSent = EmailUtility.SendEmails(
                 contactEmail, _generatedOTP, _fullName);

            if (isEmailSent)
            {
                _isOtpEnable = true;
                _somethingWrongMessage = false;
            }
            else
            {
                await _messageDialogService.ShowMessageDialogAsync(
                    ApplicationConstants.Warning,
                    ApplicationConstants.LowInternet).ConfigureAwait(true);

                _somethingWrongMessage = true;
                _isOtpEnable = false;
            }
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode navigationMode)
        {
            await base.OnNavigatedToAsync(parameter, navigationMode).ConfigureAwait(false);
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
    }
}
