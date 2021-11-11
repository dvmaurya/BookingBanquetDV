using ViewModelsAndValidation;
using ViewModelsAndValidation.Views;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace BookingBanquetDV.Source.Recover
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RecoverUser : Page, IView
    {
        public RecoverUser()
        {
            this.InitializeComponent();
            this.DataContext = ViewModelLocator.RecoverUserViewModel;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }

        public ViewModelBase ViewModel => this.DataContext as RecoverUserViewModel;
    }
}
