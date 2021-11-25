using BookingBanquetDV.Source.Camera;
using BookingServices.Interfaces;
using CommonServices.ConstantData;
using CommonServices.Extensions;
using CommonServices.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewModelsAndValidation;
using ViewModelsAndValidation.Command;
using ViewModelsAndValidation.Services;
using ViewModelsAndValidation.ViewModels;
using Windows.Storage;
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
        private bool _isViewBooking;
        private bool _isEditBooking;
        private readonly ICommand _cameraClickCommand;
        private readonly ICommand _viewPhotoClickCommand;
        private readonly ICommand _logoutCommand;
        private readonly ICommand _ViewBookingCommand;
        private readonly ICommand _editBookingCommand;
        private readonly ICommand _deleteBookingCommand;
        private readonly IBookingDataProvider _bookingDataProvider;
        private readonly INavigationService _navigationService;
        private readonly IMessageDialogService _messageDialogService;

        public WelcomeViewModel(
            ICameraViewModel cameraViewModel,
            IBookingDataProvider bookingDataProvider,
            INavigationService navigationService,
            IMessageDialogService messageDialogService)
        {
            _cameraViewModel = cameraViewModel ?? throw new ArgumentNullException("cameraViewModel");
            _bookingDataProvider = bookingDataProvider ?? throw new ArgumentNullException("bookingDataProvider");
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");
            _messageDialogService = messageDialogService ?? throw new ArgumentNullException("messageDialogService");

            _newBookingCommand = new DelegateCommand(
                async parameter => await OnClickNewBookingAsync());

            _cameraClickCommand = new DelegateCommand(
                async parameter => await OnCameraClickAsync(parameter));

            _viewPhotoClickCommand = new DelegateCommand(
                async parameter => await OnViewPhotoClickAsync(parameter));

            _logoutCommand = new DelegateCommand(
                parameter => OnLogoutClick(parameter));

            _ViewBookingCommand = new DelegateCommand(
                async parameter => await OnViewBookingClickAsync(parameter));

            _editBookingCommand = new DelegateCommand(
                async parameter => await OnEditBookingClickAsync(parameter));

            _deleteBookingCommand = new DelegateCommand(
                parameter => OnDeleteBookingClick(parameter));
        }

        private async Task OnClickNewBookingAsync()
        {
            _isViewBooking = false;
            _isEditBooking = false;

            await OnClickBookingAsync(null, _isViewBooking, _isEditBooking);
        }

        private void OnDeleteBookingClick(object parameter)
        {
            _isViewBooking = false;
            _isEditBooking = false;

            if (parameter is BookingDetails bookingDetails)
            {
                _bookingDataProvider.DeleteBooking(bookingDetails);
                _bookingDetailList.Remove(bookingDetails);

                OnPropertyChanged(() => BookingDetailList);
            }
        }

        private async Task OnEditBookingClickAsync(object parameter)
        {
            _isViewBooking = false;
            _isEditBooking = true;

            if (parameter is BookingDetails bookingDetail)
            {
                await OnClickBookingAsync(bookingDetail, _isViewBooking, _isEditBooking);
                ReloadBooking();
            }
        }

        private async Task OnViewBookingClickAsync(object parameter)
        {
            _isViewBooking = true;
            _isEditBooking = false;

            if (parameter is BookingDetails bookingDetail)
                await OnClickBookingAsync(bookingDetail, _isViewBooking, _isEditBooking);
        }

        private void OnLogoutClick(object parameter)
        {
            _bookingDetailList = null;
            _bookingDetail = null;

            _navigationService.Navigate(typeof(MainPage));
        }

        private async Task OnViewPhotoClickAsync(object parameter)
        {
            if (parameter is BookingDetails bookingDetail)
            {
                PhotoDetails photoDetails = new PhotoDetails();
                StorageFolder storageFolder =
                        await ApplicationData.Current.LocalFolder.GetFolderAsync(ApplicationConstants.Document);

                if (await storageFolder.TryGetItemAsync(bookingDetail.PhotoName) is StorageFile file)
                {
                    photoDetails.ImageSource = await PhotoUtility.GetDecrtypedPhotoAsync(file);
                    photoDetails.StorageFile = file;

                    PreviewPhoto newBooking = new PreviewPhoto(photoDetails);
                    await newBooking.ShowAsync();
                }
            }
        }

        private async Task OnCameraClickAsync(object parameter)
        {
            if (parameter is BookingDetails bookingDetail)
            {
                PhotoDetails photoDetail = await _cameraViewModel.OnClickTakeAPhotoCommandAsync();

                if (photoDetail != null)
                {
                    bookingDetail.PhotoName = photoDetail.LocalPhotoName;
                    bookingDetail.CanShowPhoto = true;
                    _bookingDataProvider.AddOrUpdateBooking(bookingDetail);

                    ReloadBooking();
                }
            }
        }

        private async Task OnClickBookingAsync(BookingDetails bookingDetail, bool isViewBooking, bool isEditBooking)
        {
            BookingParameter bookingParameter = new BookingParameter
            {
                BookingDetailList = _bookingDetailList,
                LoginDetail = _loginDetail,
                IsViewBooking = isViewBooking,
                IsEditBooking = IsEditBooking
            };

            if (bookingDetail != null)
                bookingParameter.BookingDetails = bookingDetail;

            NewBooking newBooking = new NewBooking(bookingParameter);
            await newBooking.ShowAsync();

            if (!_isViewBooking)
            {
                _bookingDetail = newBooking.BookingDetails;

                if (_bookingDetail != null)
                {
                    _bookingDetail.UserId = _loginDetail.UserId;

                    if (!_isEditBooking)
                        _bookingDetail.CreationDate = DateTime.Now.ConvertToString(
                            ApplicationConstants.DatabaseDateTimeFormat);

                    _bookingDataProvider.AddOrUpdateBooking(_bookingDetail);
                    if (_bookingDetailList == null)
                        _bookingDetailList = new ObservableCollection<BookingDetails>();

                    _bookingDetailList.Add(_bookingDetail);

                    OnPropertyChanged(() => BookingDetailList);
                }
            }
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode navigationMode)
        {
            await base.OnNavigatedToAsync(parameter, navigationMode);

            if (parameter is LoginDetail loginDetail)
            {
                _loginDetail = loginDetail;
                ReloadBooking();
            }
        }

        private void ReloadBooking()
        {
            _bookingDetailList = new ObservableCollection<BookingDetails>(
                _bookingDataProvider.GetAllBookings(_loginDetail.UserId));

            foreach (var bookingDetail in _bookingDetailList)
            {
                if (!string.IsNullOrEmpty(bookingDetail.PhotoName))
                    bookingDetail.CanShowPhoto = true;
            }

            OnPropertyChanged(() => BookingDetailList);
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

        public ICommand CameraClickCommand => _cameraClickCommand;

        public ICommand ViewPhotoClickCommand => _viewPhotoClickCommand;

        public ICommand LogoutCommand => _logoutCommand;

        public ICommand ViewBookingCommand => _ViewBookingCommand;

        public ICommand EditBookingCommand => _editBookingCommand;

        public ICommand DeleteBookingCommand => _deleteBookingCommand;

        public bool IsViewBooking
        {
            get => _isViewBooking;
            set => _isViewBooking = value;
        }

        public bool IsEditBooking
        {
            get => _isEditBooking;
            set => _isEditBooking = value;
        }
    }
}
