using BookingBanquetDV.Navigation;
using BookingBanquetDV.Source.CommonControls;
using Unity.Injection;
using ViewModelsAndValidation;
using ViewModelsAndValidation.Services;
using ViewModelsAndValidation.Views;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BookingBanquetDV
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, IView, IAppFrame
    {
        public Frame AppFrame { get { return appLoginFrame; } }

        public MainPage()
        {
            this.InitializeComponent();
            DIContainer.RegisterSingletonType<INavigationService, NavigationService>(
                 new InjectionConstructor(AppFrame));
            this.DataContext = ViewModelLocator.MainViewModel;
        }

        public ViewModelBase ViewModel => this.DataContext as MainViewModel;

        public void ShowLoadingIndicator(string contentText)
        {
            LoadingIndicator.ShowLoadingIndicator(contentText);
        }

        public void ChangeLoadingIndicatorText(string contentText)
        {
            LoadingIndicator.ChangeLoadingIndicatorText(contentText);
        }

        public void HideLoadingIndicator()
        {
            LoadingIndicator.HideLoadingIndicator();
        }
    }
}
