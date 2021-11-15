using BookingServices.Interfaces;
using CommonServices.ConstantData;
using CommonServices.Enums;
using CommonServices.Extensions;
using CommonServices.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewModelsAndValidation.Services;
using ViewModelsAndValidation.ViewModels;

namespace BookingBanquetDV.Source.Login
{
    public class LoginViewModel : ILoginViewModel
    {
        private bool _isLoginSuccess;
        private LoginDetail _loginDetail;
        private IDictionary<string, string> _userDetail;
        private readonly INavigationService _navigationService;
        private readonly IMessageDialogService _messageDialogService;
        private readonly IOfflineDatabaseProvider _offlineDatabaseProvider;

        public LoginViewModel(
            INavigationService navigationService,
            IOfflineDatabaseProvider offlineDatabaseProvider,
            IMessageDialogService messageDialogService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _offlineDatabaseProvider = offlineDatabaseProvider ?? throw new ArgumentNullException(nameof(offlineDatabaseProvider));
            _messageDialogService = messageDialogService ?? throw new ArgumentNullException(nameof(messageDialogService));
        }

        public async Task<bool> NavigateToWelcomePageAsync(object parameter)
        {
            if (parameter is IMainViewModel mainView)
            {
                _userDetail = _offlineDatabaseProvider.GetDataForLogin(
                    mainView.UserId, mainView.Password);
                
                if(_userDetail != null && _userDetail.Any())
                {
                    if (_userDetail[UsersEnum.IsActivated.ConvertToString()] == "0")
                    {
                        _isLoginSuccess = true;
                        _loginDetail = new LoginDetail
                        {
                            IsRememberChecked = mainView.IsRememberChecked,
                            UserId = mainView.UserId,
                            UserName = _userDetail[UsersEnum.FullName.ConvertToString()],
                            NumberOfLogin = "1",
                            LastLogin = DateTime.Now.ConvertToString(
                                ApplicationConstants.DatabaseDateTimeFormat)
                        };

                        SaveLoginDetails(_loginDetail);
                        _navigationService.Navigate(typeof(WelcomePage), _loginDetail);
                    }
                    else
                    {
                        _isLoginSuccess = false;
                        await _messageDialogService.ShowMessageDialogAsync(
                            ApplicationConstants.Error,
                            ApplicationConstants.AccountNotActivated).ConfigureAwait(true);
                    }
                }
                else
                {
                    _isLoginSuccess = false;
                    await _messageDialogService.ShowMessageDialogAsync(
                        ApplicationConstants.Error,
                        ApplicationConstants.CredentialWrong).ConfigureAwait(true);
                }
            }

            return _isLoginSuccess;
        }

        private void SaveLoginDetails(LoginDetail loginDetail)
        {
            _offlineDatabaseProvider.AddOrUpdateLoginDetail(loginDetail);
        }
    }
}
