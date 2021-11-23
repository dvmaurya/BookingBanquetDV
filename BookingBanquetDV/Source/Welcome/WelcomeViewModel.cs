using CommonServices.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewModelsAndValidation;
using ViewModelsAndValidation.Command;
using ViewModelsAndValidation.ViewModels;
using Windows.UI.Xaml.Navigation;

namespace BookingBanquetDV.Source.Welcome
{
    public class WelcomeViewModel : ValidationViewModelBase, IWelcomeViewModel
    {
        private LoginDetail _loginDetail;
        private BookingDetails _bookingDetail;
        private ObservableCollection<BookingDetails> _bookingDetailList;

        private readonly ICommand _newBookingCommand;
        private ICameraViewModel _cameraViewModel;
        private readonly 

        public WelcomeViewModel(
            ICameraViewModel cameraViewModel)
        {
            _cameraViewModel = cameraViewModel ?? throw new ArgumentNullException("cameraViewModel");

            _newBookingCommand = new DelegateCommand(
                async parameter => await OnClickNewBookingAsync());
        }

        private async Task OnClickNewBookingAsync()
        {
            NewBooking newBooking = new NewBooking(_loginDetail);
            await newBooking.ShowAsync();

            _bookingDetail = newBooking.BookingDetails;

            if (_bookingDetail != null)
            {
                if (_bookingDetailList == null)
                    _bookingDetailList = new ObservableCollection<BookingDetails>();

                _bookingDetailList.Add(_bookingDetail);

                //OnPropertyChanged(() => BookingDetailList);
            }
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode navigationMode)
        {
            await base.OnNavigatedToAsync(parameter, navigationMode);

            if (parameter is LoginDetail loginDetail)
            {
                _loginDetail = loginDetail;
            }
        }

        public ICommand NewBookingCommand => _newBookingCommand;

        public BookingDetails BookingDetail
        {
            get => _bookingDetail;
            set => _bookingDetail = value;
        }

        public ICameraViewModel CameraViewModel
        {
            get => _cameraViewModel;
            set => SetValue(() => CameraViewModel, ref _cameraViewModel, value);
        }

        public ObservableCollection<BookingDetails> BookingDetailList => _bookingDetailList;
    }
}
