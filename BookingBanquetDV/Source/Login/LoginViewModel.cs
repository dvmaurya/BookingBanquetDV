using BookingBanquetDV.Source.Welcome;
using BookingServices.Interfaces;
using CommonServices.Analytcis;
using CommonServices.ConstantData;
using CommonServices.Enums;
using CommonServices.Extensions;
using CommonServices.Model;
using CommonServices.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly IAnalyticsManager _analyticsManager;

        public LoginViewModel(
            INavigationService navigationService,
            IOfflineDatabaseProvider offlineDatabaseProvider,
            IMessageDialogService messageDialogService,
            IAnalyticsManager analyticsManager)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");
            _offlineDatabaseProvider = offlineDatabaseProvider ?? throw new ArgumentNullException("offlineDatabaseProvider");
            _messageDialogService = messageDialogService ?? throw new ArgumentNullException("messageDialogService");
            _analyticsManager = analyticsManager ?? throw new ArgumentNullException("analyticsManager");
        }

        public async Task<bool> NavigateToWelcomePageAsync(object parameter)
        {
            if (parameter is IMainViewModel mainView)
            {
                _userDetail = _offlineDatabaseProvider.GetDataForLogin(
                    mainView.UserId, mainView.Password);
                
                if(_userDetail != null && _userDetail.Any())
                {
                    if (_userDetail[UsersEnum.IsAdmin.ConvertToString()] == "0")
                    {
                        _analyticsManager.TraceEvents(
                            analyticsEvent: $"{AnalyticConstants.Application} ",
                            action: $"{AnalyticConstants.Login} " +
                                $"{AnalyticConstants.Success} ",
                            label: $"{AnalyticConstants.User} " +
                                    $"{AnalyticConstants.Login} " +
                                    $"{AnalyticConstants.Successfully} ");

                        _isLoginSuccess = true;
                        _loginDetail = new LoginDetail
                        {
                            IsRememberChecked = mainView.IsRememberChecked,
                            UserId = mainView.UserId,
                            MobileNo = _userDetail[UsersEnum.Mobile.ConvertToString()],
                            NumberOfLogin = "1",
                            LastLogin = DateTime.Now.ConvertToString(
                                ApplicationConstants.DatabaseDateTimeFormat),
                            EmailId = _userDetail[UsersEnum.EmailId.ConvertToString()]
                        };

                        Helper.SetLocalSetting(ApplicationConstants.CurrentUserEmailId, _userDetail[UsersEnum.EmailId.ConvertToString()]);
                        Helper.SetLocalSetting(ApplicationConstants.CurrentUserMobileNo, _userDetail[UsersEnum.Mobile.ConvertToString()]);

                        SaveLoginDetails(_loginDetail);
                        _navigationService.Navigate(typeof(WelcomePage), _loginDetail);
                    }
                    else
                    {
                        _analyticsManager.TraceEvents(
                            analyticsEvent: $"{AnalyticConstants.Application} ",
                            action: $"{AnalyticConstants.Login} " +
                                $"{AnalyticConstants.Failed} ",
                            label: $"{AnalyticConstants.User} " +
                                    $"{AnalyticConstants.Account} " +
                                    $"not {AnalyticConstants.Verified} ");

                        _isLoginSuccess = false;
                        await _messageDialogService.ShowMessageDialogAsync(
                            ApplicationConstants.Error,
                            ApplicationConstants.AccountNotActivated).ConfigureAwait(true);
                    }
                }
                else
                {
                    _analyticsManager.TraceEvents(
                            analyticsEvent: $"{AnalyticConstants.Application} ",
                            action: $"{AnalyticConstants.Login} " +
                                $"{AnalyticConstants.Failed} ",
                            label: $"{AnalyticConstants.User} " +
                                    $"{AnalyticConstants.Credential} " +
                                    $"not {AnalyticConstants.Wrong} ");

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
