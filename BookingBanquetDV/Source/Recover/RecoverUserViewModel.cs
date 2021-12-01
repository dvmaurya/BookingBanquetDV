using BookingBanquetDV.Utility;
using BookingBanquetDV.Validation;
using BookingServices.Interfaces;
using CommonServices.ConstantData;
using CommonServices.Enums;
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

namespace BookingBanquetDV.Source.Recover
{
    public class RecoverUserViewModel : ValidationViewModelBase, IRecoverUserViewModel
    {
        private string _emailId;
        private string _mobileNo;
        private string _oTP;
        private bool _isOtpEnable;
        private bool _invalidCredential;
        private string _generatedOTP;
        private bool _somethingWrongMessage;
        private bool _isRecoverUIVisible;
        private bool _isNewPasswordVisible;
        private string _password;
        private string _confirmPassword;
        private IDictionary<string, string> _userDetails;
        private bool _passwordUpdateFailed;
        private readonly ICommand _changePasswordCommand;
        private readonly ICommand _getOTPCommand;
        private readonly ICommand _recoverCommand;
        private readonly ICommand _loginCommand;
        private readonly INavigationService _navigationService;
        private readonly IMessageDialogService _messageDialogService;
        private readonly IOfflineDatabaseProvider _offlineDatabaseProvider;

        public RecoverUserViewModel(
            INavigationService navigationService,
            IMessageDialogService messageDialogService,
            IOfflineDatabaseProvider offlineDatabaseProvider)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _messageDialogService = messageDialogService ?? throw new ArgumentNullException(nameof(messageDialogService));
            _offlineDatabaseProvider = offlineDatabaseProvider ?? throw new ArgumentNullException(nameof(offlineDatabaseProvider));

            _isRecoverUIVisible = true;
            _recoverCommand = new DelegateCommand(parameter => OnClickRecover());
            _loginCommand = new DelegateCommand(parameter => OnClickLogin());
            _getOTPCommand = new DelegateCommand(async parameter => await OnClickGetOTPAsync());
            _changePasswordCommand = new DelegateCommand(async parameter => await OnClickChangePasswordAsync());
        }

        private async Task OnClickChangePasswordAsync()
        {
            if (ValidateProperties())
            {
                Users users = new Users
                {
                    Id = _userDetails[UsersEnum.Id.ConvertToString()].ToInt32(),
                    EmailId = _userDetails[UsersEnum.EmailId.ConvertToString()],
                    FullName = _userDetails[UsersEnum.FullName.ConvertToString()],
                    UserId = _userDetails[UsersEnum.UserId.ConvertToString()],
                    Mobile = _userDetails[UsersEnum.Mobile.ConvertToString()],
                    Password = _password,
                    CreationDate = _userDetails[UsersEnum.CreationDate.ConvertToString()],
                    IsActivate = _userDetails[UsersEnum.IsActivated.ConvertToString()] == "0"
                                ? false : true,
                    PasswordChanged = _userDetails[UsersEnum.PasswordChanged.ConvertToString()].ToInt32() + 1
                };

                bool isPasswordUpdated = _offlineDatabaseProvider.AddOrUpdateUsers(users);

                if (isPasswordUpdated)
                {
                    await _messageDialogService.ShowMessageDialogAsync(
                        ApplicationConstants.Success,
                        ApplicationConstants.PasswordUpdated).ConfigureAwait(true);

                    _navigationService.Navigate(typeof(MainPage));
                }
                else
                {
                    _passwordUpdateFailed = true;

                    OnPropertyChanged(() => PasswordUpdateFailed);
                }
            }
        }

        private async Task OnClickGetOTPAsync()
        {
            if (ValidateProperties())
            {
                _userDetails = _offlineDatabaseProvider.GetDataForOTP(_emailId, _mobileNo);

                if (_userDetails != null && _userDetails.Any())
                {
                    _invalidCredential = false;
                    _generatedOTP = Utilities.GenerateOTP();

                    EmailParameter emailParameter = new EmailParameter
                    {
                        EmailAddress = _emailId,
                        IsRecovery = true,
                        Otp = _generatedOTP,
                        UserName = _userDetails[UsersEnum.FullName.ToString()]
                    };

                    App.ShowLoadingIndicator(ApplicationConstants.SendingEmail);
                    bool isEmailSent = await EmailUtility.SendEmailsAsync(emailParameter);
                    App.HideLoadingIndicator();
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

                    OnPropertyChanged(() => IsOtpEnable);
                    OnPropertyChanged(() => SomethingWrongMessage);
                }
                else
                {
                    _invalidCredential = true;
                }

                OnPropertyChanged(() => InvalidCredential);
            }
        }

        private void OnClickLogin()
        {
            _navigationService.Navigate(typeof(MainPage));
        }

        private void OnClickRecover()
        {
            if (ValidateProperties())
            {
                if (_oTP.FormatStringEquals(_generatedOTP))
                {
                    _isRecoverUIVisible = false;
                    _isNewPasswordVisible = true;
                }

                OnPropertyChanged(() => IsRecoverUIVisible);
                OnPropertyChanged(() => IsNewPasswordVisible);
            }
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

        [OTPValidationAttribute(ErrorMessage = ApplicationConstants.InvalidOTP)]
        public string OTP
        {
            get => _oTP;
            set => _oTP = value;
        }

        public bool IsOtpEnable
        {
            get => _isOtpEnable;
            set => _isOtpEnable = value;
        }

        public string GeneratedOTP
        {
            get => _generatedOTP;
            set => _generatedOTP = value;
        }

        public bool SomethingWrongMessage
        {
            get => _somethingWrongMessage;
            set => _somethingWrongMessage = value;
        }

        public bool InvalidCredential
        {
            get => _invalidCredential;
            set => _invalidCredential = value;
        }

        public ICommand RecoverCommand => _recoverCommand;
        public ICommand LoginCommand => _loginCommand;
        public ICommand GetOTPCommand => _getOTPCommand;
        public ICommand ChangePasswordCommand => _changePasswordCommand;

        public bool IsRecoverUIVisible
        {
            get => _isRecoverUIVisible;
            set => _isRecoverUIVisible = value;
        }

        public bool IsNewPasswordVisible
        {
            get => _isNewPasswordVisible;
            set => _isNewPasswordVisible = value;
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

        public bool PasswordUpdateFailed
        {
            get => _passwordUpdateFailed;
            set => _passwordUpdateFailed = value;
        }
    }
}
