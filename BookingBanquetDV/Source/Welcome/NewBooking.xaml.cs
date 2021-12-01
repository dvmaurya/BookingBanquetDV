using BookingBanquetDV.Utility;
using CommonServices.ConstantData;
using CommonServices.Enums;
using CommonServices.Extensions;
using CommonServices.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using ViewModelsAndValidation.ViewModels;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace BookingBanquetDV.Source.Welcome
{
    public sealed partial class NewBooking : ContentDialog
    {
        private BookingDetails _bookingDetails;
        private bool _isValidationSuccess;
        private BookingParameter _bookingParameter;

        public event EventHandler<AddBookingEventArgs> BookingEvent;

        public NewBooking(BookingParameter bookingParameter)
        {
            this.InitializeComponent();
            this.DataContext = ViewModelLocator.NewBookingViewModel;

            _bookingParameter = bookingParameter;
            _bookingDetails = new BookingDetails();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if(_bookingParameter != null && !_bookingParameter.IsViewBooking)
            {
                if (sender != null &&
                    args != null &&
                    this.DataContext is INewBookingViewModel viewModel)
                {
                    viewModel.SaveCommand.Execute(null);
                    _isValidationSuccess = viewModel.IsValidationSuccess;

                    if (_isValidationSuccess)
                    {
                        _bookingDetails.Id = viewModel.Id;
                        _bookingDetails.CustomerName = !string.IsNullOrEmpty(viewModel.CustomerName) ? viewModel.CustomerName : string.Empty;

                        _bookingDetails.BookingDate = !string.IsNullOrEmpty(viewModel.BookingDate.ConvertToString()) ?
                            viewModel.BookingDate.ConvertToString(
                            ApplicationConstants.DatabaseDateFormat) : string.Empty;

                        _bookingDetails.CreationDate = viewModel.CreationDate.ConvertToString(ApplicationConstants.DatabaseDateTimeFormat);
                        _bookingDetails.BookedBy = !string.IsNullOrEmpty(viewModel.BookedBy) ? viewModel.BookedBy : string.Empty;
                        _bookingDetails.MobileNo = !string.IsNullOrEmpty(viewModel.MobileNo) ? viewModel.MobileNo : string.Empty;
                        _bookingDetails.EmailId = !string.IsNullOrEmpty(viewModel.EmailId) ? viewModel.EmailId : string.Empty;
                        _bookingDetails.TotalRooms = !string.IsNullOrEmpty(viewModel.TotalRooms) ? viewModel.TotalRooms : string.Empty;
                        _bookingDetails.TotalFlours = !string.IsNullOrEmpty(viewModel.TotalFlours) ? viewModel.TotalFlours : string.Empty;
                        _bookingDetails.TotalAmount = !string.IsNullOrEmpty(viewModel.TotalAmount) ? viewModel.TotalAmount : string.Empty;
                        _bookingDetails.Advanced = !string.IsNullOrEmpty(viewModel.Advanced) ? viewModel.Advanced : string.Empty;
                        _bookingDetails.Balance = !string.IsNullOrEmpty(viewModel.Balance) ? viewModel.Balance : string.Empty;
                        _bookingDetails.Confirmation = !string.IsNullOrEmpty(viewModel.SelecteConfirmation) ? viewModel.SelecteConfirmation : "Not Decided";
                        _bookingDetails.PhotoName = !string.IsNullOrEmpty(viewModel.PhotoName) ? viewModel.PhotoName : string.Empty;
                        _bookingDetails.IsEmailSent = viewModel.IsEmailSent;

                        this.Hide();
                        BookingEvent?.Invoke(
                            this,
                            new AddBookingEventArgs(_bookingDetails, PreviewDialogAction.Submit));
                    }
                    else
                    {
                        args.Cancel = true;
                    }
                }
            }
            else
            {
                this.Hide();
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (sender != null && args != null)
            {
                _bookingDetails = null;
                this.Hide();
            }
        }

        private void ContentDialog_Loading(Windows.UI.Xaml.FrameworkElement sender, object args)
        {
            if (sender != null &&
                this.DataContext is INewBookingViewModel viewModel)
            {
                if(_bookingParameter != null)
                {
                    if (_bookingParameter.IsViewBooking)
                        BookingDialog.Title = "View booking";
                    else if (_bookingParameter.IsEditBooking)
                        BookingDialog.Title = "Update booking";
                    else
                        BookingDialog.Title = "Add new booking";
                }
                viewModel.BookingParameter = _bookingParameter;
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
