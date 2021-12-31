using BookingBanquetDV.Source.Camera;
using CommonServices.Analytcis;
using CommonServices.ConstantData;
using CommonServices.Model;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewModelsAndValidation;
using ViewModelsAndValidation.Command;
using ViewModelsAndValidation.Services;
using ViewModelsAndValidation.ViewModels;
using Windows.Storage;
using Windows.UI.Xaml.Navigation;

namespace BookingBanquetDV.Source.Welcome.PrintOut
{
    public class PrintLoadingViewModel : ViewModelBase, IPrintLoadingViewModel
    {
        private BookingDetails _bookingDetails;
        private CustomerPhotoDetail _customerPhoto;
        private LoginDetail _loginDetail;

        private readonly ICommand _printCommand;

        private readonly INavigationService _nativationService;
        private readonly IAnalyticsManager _analyticsManager;

        public PrintLoadingViewModel(
            INavigationService navigationService,
            IAnalyticsManager analyticsManager)
        {
            _nativationService = navigationService ?? throw new ArgumentNullException("navigationService");
            _analyticsManager = analyticsManager ?? throw new ArgumentNullException("analyticsManager");

            _printCommand = new DelegateCommand(parameter => OnPrintPage());
        }

        private void OnPrintPage()
        {
            _nativationService.Navigate(typeof(WelcomePage), _loginDetail);
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode navigationMode)
        {
            await base.OnNavigatedToAsync(parameter, navigationMode);

            if (parameter is NavigationParameter navigation)
            {
                _bookingDetails = navigation.BookingDetails;
                _customerPhoto = navigation.CustomerPhoto;
                _loginDetail = navigation.LoginDetail;

                if (_customerPhoto != null)
                    await SetPhotoAsync();

                _analyticsManager.TraceEvents(
                    analyticsEvent: $"{AnalyticConstants.Printout}",
                    action: $"{AnalyticConstants.User} " +
                        $"{AnalyticConstants.Clicked} " +
                        $" on {AnalyticConstants.Printout}");
            }
        }

        private async Task SetPhotoAsync()
        {
            if (_customerPhoto != null && !string.IsNullOrEmpty(_customerPhoto.PhotoName))
            {
                StorageFolder storageFolder = await ApplicationData.Current.LocalFolder.
                    GetFolderAsync(ApplicationConstants.CustomerPhoto);

                if (await storageFolder.TryGetItemAsync(_customerPhoto.PhotoName) is StorageFile file)
                    _customerPhoto.ImageSource = await PhotoUtility.GetDecrtypedPhotoAsync(file, ApplicationConstants.CustomerPhoto);

                OnPropertyChanged(() => CustomerPhoto);
            }
        }

        public BookingDetails BookingDetails
        {
            get => _bookingDetails;
            set => _bookingDetails = value;
        }

        public LoginDetail LoginDetail
        {
            get => _loginDetail;
            set => _loginDetail = value;
        }

        public CustomerPhotoDetail CustomerPhoto
        {
            get => _customerPhoto;
            set => _customerPhoto = value;
        }

        public ICommand PrintCommand => _printCommand;
    }
}