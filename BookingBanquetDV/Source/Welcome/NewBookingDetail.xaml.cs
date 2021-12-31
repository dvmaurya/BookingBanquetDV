using CommonServices.Extensions;
using ViewModelsAndValidation;
using ViewModelsAndValidation.Views;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace BookingBanquetDV.Source.Welcome
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewBookingDetail : Page, IView
    {
        public NewBookingDetail()
        {
            this.InitializeComponent();
            this.DataContext = ViewModelLocator.NewBookingViewModel;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }

        public ViewModelBase ViewModel => this.DataContext as NewBookingViewModel;

        private void TextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            int totalAmount = !string.IsNullOrEmpty(TotalAmount.Text) ? TotalAmount.Text.ToInt32() : 0;
            int advnce = !string.IsNullOrEmpty(Advance.Text) ? Advance.Text.ToInt32() : 0;

            RemainingAmount.Text = (totalAmount - advnce).ConvertToString();

            if (sender != null && e != null) { }
        }
    }
}
