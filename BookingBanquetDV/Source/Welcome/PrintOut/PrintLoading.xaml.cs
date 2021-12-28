using CommonServices.Printout;
using ViewModelsAndValidation;
using ViewModelsAndValidation.Services;
using ViewModelsAndValidation.Views;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace BookingBanquetDV.Source.Welcome.PrintOut
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PrintLoading : Page, IView
    {
        private PrintHelper _printHelper;
        private readonly INavigationService _navigationService;

        public ViewModelBase ViewModel => this.DataContext as PrintLoadingViewModel;

        public PrintLoading()
        {
            this.InitializeComponent();
            this.DataContext = ViewModelLocator.PrintLoadingViewModel;
            _navigationService = DIContainer.GetInstance<INavigationService>();
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }

        private async void OnPrintButtonClick(object sender, RoutedEventArgs e)
        {
            _printHelper.PrintDone +=
                (senders, eventArgs) => PrintCompleted(sender, eventArgs);

            await _printHelper.ShowPrintUIAsync();
        }

        private void PrintCompleted(object sender, bool eventArgs)
        {
            if (eventArgs && this.DataContext is PrintLoadingViewModel printLoading)
                _navigationService.Navigate(typeof(WelcomePage), printLoading.LoginDetail);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _printHelper = new PrintHelper(this);
            _printHelper.RegisterForPrinting();
            _printHelper.PreparePrintContent(new PageToPrint());
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (_printHelper != null)
                _printHelper.UnregisterForPrinting();
        }
    }
}

