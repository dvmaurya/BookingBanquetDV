using CommonServices.Extensions;
using CommonServices.Model;
using ViewModelsAndValidation.ViewModels;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace BookingBanquetDV.Source.Welcome
{
    public sealed partial class NewBooking : ContentDialog
    {
        private BookingDetails _bookingDetails;
        private LoginDetail _loginDetail;

        public NewBooking(LoginDetail loginDetail)
        {
            this.InitializeComponent();
            this.DataContext = ViewModelLocator.NewBookingViewModel;
            _loginDetail = loginDetail;
            _bookingDetails = new BookingDetails();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (sender != null &&
                args != null &&
                this.DataContext is INewBookingViewModel newBookingViewModel)
            {
                _bookingDetails.CustomerName = !string.IsNullOrEmpty(newBookingViewModel.CustomerName) ? newBookingViewModel.CustomerName : string.Empty;
                _bookingDetails.BookingDate = !string.IsNullOrEmpty(newBookingViewModel.BookingDate.ConvertToString()) ? newBookingViewModel.BookingDate.ConvertToString() : string.Empty;
                _bookingDetails.BookedBy = !string.IsNullOrEmpty(newBookingViewModel.BookedBy) ? newBookingViewModel.BookedBy : string.Empty;
                _bookingDetails.NumberOfRooms = !string.IsNullOrEmpty(newBookingViewModel.NumberOfRooms) ? newBookingViewModel.NumberOfRooms : string.Empty;
                _bookingDetails.NumberOfFlours = !string.IsNullOrEmpty(newBookingViewModel.NumberOfFlours) ? newBookingViewModel.NumberOfFlours : string.Empty;
                _bookingDetails.TotalAmount = !string.IsNullOrEmpty(newBookingViewModel.TotalAmount) ? newBookingViewModel.TotalAmount : string.Empty;
                _bookingDetails.Advanced = !string.IsNullOrEmpty(newBookingViewModel.Advanced) ? newBookingViewModel.Advanced : string.Empty;
                _bookingDetails.Balance = !string.IsNullOrEmpty(newBookingViewModel.Balance) ? newBookingViewModel.Balance : string.Empty;
                _bookingDetails.Confirmation = !string.IsNullOrEmpty(newBookingViewModel.SelecteConfirmation) ? newBookingViewModel.SelecteConfirmation : string.Empty;
            }

            this.Hide();
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (sender != null && args != null)
            {
                _bookingDetails = null;
                this.Hide();
            }
        }

        public BookingDetails BookingDetails => _bookingDetails;

        private void ContentDialog_Loading(Windows.UI.Xaml.FrameworkElement sender, object args)
        {
            if (sender != null &&
                this.DataContext is INewBookingViewModel viewModel)
            {
                viewModel.LoginDetail = _loginDetail;
                viewModel.DialogLoadingCommand.Execute(null);
            }

            if (args != null) { }
        }

        private void TextBox_KeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            int totalAmount = !string.IsNullOrEmpty(TotalAmount.Text) ? TotalAmount.Text.ToInt32() : 0;
            int advnce = !string.IsNullOrEmpty(Advance.Text) ? Advance.Text.ToInt32() : 0;

            RemainingAmount.Text = (totalAmount - advnce).ConvertToString();
        }
    }
}
