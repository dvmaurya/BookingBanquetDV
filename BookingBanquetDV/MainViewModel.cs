using BookingBanquetDV.Source.Recover;
using BookingBanquetDV.Source.Register;
using BookingServices.Interfaces;
using CommonServices.Analytcis;
using CommonServices.ConstantData;
using CommonServices.Extensions;
using CommonServices.Model.DbModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewModelsAndValidation;
using ViewModelsAndValidation.Command;
using ViewModelsAndValidation.Services;
using ViewModelsAndValidation.ViewModels;

namespace BookingBanquetDV
{
    public class MainViewModel : ValidationViewModelBase, IMainViewModel
    {
        private string _userId = "8318412422";
        private string _password = "123";
        private bool _isRememberChecked;
        private bool _isCredentialWrong;
        private readonly ICommand _loginCommand;
        private readonly ICommand _signupCommand;
        private readonly ICommand _forgotPasswordCommand;
        
        private readonly INavigationService _navigationService;
        private readonly ILoginViewModel _loginViewModel;
        private readonly IOfflineDatabaseProvider _offlineDatabaseProvider;
        private readonly IAnalyticsManager _analyticsManager;

        public MainViewModel(
            INavigationService navigationService,
            ILoginViewModel loginViewModel,
            IOfflineDatabaseProvider offlineDatabaseProvider,
            IAnalyticsManager analyticsManager)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");
            _loginViewModel = loginViewModel ?? throw new ArgumentNullException("loginViewModel");
            _offlineDatabaseProvider = offlineDatabaseProvider ?? throw new ArgumentNullException("offlineDatabaseProvider");
            _analyticsManager = analyticsManager ?? throw new ArgumentNullException("analyticsManager");

            _loginCommand = new DelegateCommand(async parameter => await OnClickLoginAsync(parameter));
            _signupCommand = new DelegateCommand(parameter => OnClickSignup());
            _forgotPasswordCommand = new DelegateCommand(parameter => OnClickForgotPassword());
        }

        public void GetPreviousLoginDetails()
        {
            IDictionary<string, string> loginDetail = _offlineDatabaseProvider.GetPreviousLoginDetail();

            if(loginDetail != null && loginDetail.Any())
            {
                _isRememberChecked = 
                    loginDetail[nameof(LoginDetail.IsRememberChecked)].ToInt32() == 1
                    ? true : false;

                if(_isRememberChecked)
                    _userId = loginDetail[nameof(LoginDetail.UserId)];

                OnPropertyChanged(() => UserId);
                OnPropertyChanged(() => IsRememberChecked);
            }
        }

        private void OnClickForgotPassword()
        {
            _navigationService.Navigate(typeof(RecoverUser));
        }

        private void OnClickSignup()
        {
            App.ShowLoadingIndicator(ApplicationConstants.Loading);
            _navigationService.Navigate(typeof(Signup));
            App.HideLoadingIndicator();
        }

        private async Task OnClickLoginAsync(object parameter)
        {
            if (ValidateProperties())
            {
                _isCredentialWrong = await _loginViewModel.NavigateToWelcomePageAsync(parameter);

                _isCredentialWrong = !_isCredentialWrong;
                OnPropertyChanged(() => IsCredentialWrong);
            }
        }

        public ICommand LoginCommand => _loginCommand;

        public ICommand SignupCommand => _signupCommand;

        public ICommand ForgotPasswordCommand => _forgotPasswordCommand;

        [Required(ErrorMessage = ApplicationConstants.PasswordRequired)]
        public string Password
        {
            get => _password;
            set => _password = value;
        }

        [Required(ErrorMessage = ApplicationConstants.UserNameRequired)]
        public string UserId
        {
            get => _userId;
            set => _userId = value;
        }

        public bool IsRememberChecked
        {
            get => _isRememberChecked;
            set => _isRememberChecked = value;
        }

        public bool IsCredentialWrong
        {
            get => _isCredentialWrong;
            set => _isCredentialWrong = value;
        }
    }
}
