using BookingBanquetDV.Validation;
using BookingServices.Interfaces;
using CommonServices.ConstantData;
using CommonServices.Extensions;
using CommonServices.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewModelsAndValidation;
using ViewModelsAndValidation.Command;
using ViewModelsAndValidation.Services;
using ViewModelsAndValidation.ViewModels;
using Windows.UI.Xaml.Navigation;
using System.Linq;

namespace BookingBanquetDV.Source.Register
{
    public class SignupViewModel : ValidationViewModelBase, ISignupViewModel
    {
        private string _fullName;
        private string _emailId;
        private string _mobileNo;
        private string _password;
        private string _confirmPassword;
        private List<String> _existUsers;
        private bool _isEmailExist;
        private bool _isMobileExist;
        private readonly ICommand _signupCommand;
        private readonly ICommand _loginCommand;
        private readonly IMessageDialogService _messageDialogService;
        private readonly INavigationService _navigationService;
        private readonly IOfflineDatabaseProvider _offlineDatabaseProvider;

        public SignupViewModel(
            INavigationService navigationService,
            IOfflineDatabaseProvider offlineDatabaseProvider,
            IMessageDialogService messageDialogService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");
            _offlineDatabaseProvider = offlineDatabaseProvider ?? throw new ArgumentNullException("offlineDatabaseProvider");
            _messageDialogService = messageDialogService ?? throw new ArgumentNullException("messageDialogService");

            _signupCommand = new DelegateCommand(
                parameter => OnSignUpClickAsync(parameter));

            _loginCommand = new DelegateCommand(
                parameter => OnLoginClick(parameter));
        }

        private void OnLoginClick(object parameter)
        {
            _navigationService.Navigate(typeof(MainPage));
        }

        private async Task OnSignUpClickAsync(object parameter)
        {
            if (!string.IsNullOrEmpty(_emailId) &&
                !string.IsNullOrEmpty(_mobileNo))
            {
                _existUsers = _offlineDatabaseProvider.GetAllUsers(ApplicationConstants.Signup);

                if (_existUsers != null && _existUsers.Any())
                {
                    foreach (string existUser in _existUsers)
                    {
                        if (existUser.FormatStringEquals(_emailId))
                        {
                            _isEmailExist = true;
                            break;
                        }
                        else if(existUser.FormatStringEquals(_mobileNo))
                        {
                            _isMobileExist = true;
                            break;
                        }
                    }
                }
            }

            if (ValidateProperties())
            {
                Users users = new Users
                {
                    EmailId = _emailId,
                    FullName = _fullName,
                    Mobile = _mobileNo,
                    Password = _password,
                    UserId = _emailId,
                    CreationDate = DateTime.Now.ConvertToString(
                        ApplicationConstants.DatabaseDateTimeFormat),
                    IsActivate = false
                };

                bool isSaved = _offlineDatabaseProvider.AddOrUpdateUsers(users);

                if(isSaved)
                {
                    _fullName = _emailId = _mobileNo = _password = _confirmPassword = string.Empty;

                    await _messageDialogService.ShowMessageDialogAsync(
                        string.Empty,
                        ApplicationConstants.SignupSuccess).ConfigureAwait(true);

                    _navigationService.Navigate(typeof(MainPage));
                }
            }

            _isMobileExist = false;
            _isEmailExist = false;
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode navigationMode)
        {
            await base.OnNavigatedToAsync(parameter, navigationMode).ConfigureAwait(false);
        }

        [Required(ErrorMessage = ApplicationConstants.NameRequired)]
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

        public ICommand SignupCommand => _signupCommand;
        public ICommand LoginCommand => _loginCommand;
    }
}
