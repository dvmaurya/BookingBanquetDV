using CommonServices.Model;
using ViewModelsAndValidation.ViewModels;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace BookingBanquetDV.Source.Welcome.Controls
{
    public sealed partial class ViewBookingContent : ContentDialog
    {
        private BookingDetails _bookingDetails;
        private CustomerPhotoDetail _customerPhoto;

        public ViewBookingContent(BookingDetails bookingDetails, CustomerPhotoDetail customerPhoto)
        {
            this.InitializeComponent();
            _bookingDetails = bookingDetails;
            _customerPhoto = customerPhoto;
            this.DataContext = ViewModelLocator.ViewBookingViewModel;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            this.Hide();
        }

        private void ContentDialog_Loading(Windows.UI.Xaml.FrameworkElement sender, object args)
        {
            if (sender != null &&
                this.DataContext is IViewBookingViewModel viewModel)
            {
                viewModel.BookingDetails = _bookingDetails;
                viewModel.CustomerPhoto = _customerPhoto;
                viewModel.OnLoadDialogCommand.Execute(null);
            }

            if (args != null) { }
        }
    }
}
