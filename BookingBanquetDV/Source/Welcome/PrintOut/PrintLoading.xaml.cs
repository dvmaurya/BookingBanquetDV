using CommonServices.Analytcis;
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
        private readonly IAnalyticsManager _analyticsManager;

        public ViewModelBase ViewModel => this.DataContext as PrintLoadingViewModel;

        public PrintLoading()
        {
            this.InitializeComponent();
            this.DataContext = ViewModelLocator.PrintLoadingViewModel;
            _navigationService = DIContainer.GetInstance<INavigationService>();
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            _analyticsManager = DIContainer.GetInstance<IAnalyticsManager>();
        }

        private async void OnPrintButtonClick(object sender, RoutedEventArgs e)
        {
            _printHelper.PrintDone +=
                (senders, eventArgs) => PrintCompleted(sender, eventArgs);

            await _printHelper.ShowPrintUIAsync();

            if(e != null) { }
        }

        private void PrintCompleted(object sender, bool eventArgs)
        {
            if (eventArgs && this.DataContext is PrintLoadingViewModel printLoading)
            {
                _analyticsManager.TraceEvents(
                    analyticsEvent: $"{AnalyticConstants.Printout}",
                    action: $"{AnalyticConstants.Printout} " +
                        $"{AnalyticConstants.Completed} " +
                        $"{AnalyticConstants.Successfully}");

                _navigationService.Navigate(typeof(WelcomePage), printLoading.LoginDetail);
            }

            if (sender != null) { }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _printHelper = new PrintHelper(this, _analyticsManager);
            _printHelper.RegisterForPrinting();
            _printHelper.PreparePrintContent(new PageToPrint());

            if (e != null) { }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (_printHelper != null)
                _printHelper.UnregisterForPrinting();

            if (e != null) { }
        }
    }
}

