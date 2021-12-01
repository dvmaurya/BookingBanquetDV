using BookingBanquetDV.Source.Camera;
using BookingBanquetDV.Source.Welcome.Controls;
using BookingBanquetDV.Utility;
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
using Windows.UI.Popups;
using Windows.UI.Xaml.Navigation;

namespace BookingBanquetDV.Source.Welcome
{
    public class WelcomeViewModel : ValidationViewModelBase, IWelcomeViewModel
    {
        private LoginDetail _loginDetail;
        private BookingDetails _bookingDetail;
        private ObservableCollection<BookingDetails> _masterBookingDetailList;
        private ObservableCollection<BookingDetails> _bookingDetailList;

        private readonly ICommand _newBookingCommand;
        private ICameraViewModel _cameraViewModel;
        private bool _isViewBooking;
        private bool _isEditBooking;
        private readonly ICommand _bookedByDetailCommand;
        private readonly ICommand _cameraClickCommand;
        private readonly ICommand _viewPhotoClickCommand;
        private readonly ICommand _logoutCommand;
        private readonly ICommand _ViewBookingCommand;
        private readonly ICommand _editBookingCommand;
        private readonly ICommand _deleteBookingCommand;
        private readonly IBookingDataProvider _bookingDataProvider;
        private readonly INavigationService _navigationService;
        private readonly IMessageDialogService _messageDialogService;
        private readonly IOfflineDatabaseProvider _offlineDatabaseProvider;
        private readonly IContentDialogService _contentDialogService;

        public WelcomeViewModel(
            ICameraViewModel cameraViewModel,
            IBookingDataProvider bookingDataProvider,
            INavigationService navigationService,
            IMessageDialogService messageDialogService,
            IOfflineDatabaseProvider offlineDatabaseProvider,
            IContentDialogService contentDialogService)
        {
            _cameraViewModel = cameraViewModel ?? throw new ArgumentNullException("cameraViewModel");
            _bookingDataProvider = bookingDataProvider ?? throw new ArgumentNullException("bookingDataProvider");
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");
            _messageDialogService = messageDialogService ?? throw new ArgumentNullException("messageDialogService");
            _offlineDatabaseProvider = offlineDatabaseProvider ?? throw new ArgumentNullException("offlineDatabaseProvider");
            _contentDialogService = contentDialogService ?? throw new ArgumentNullException("contentDialogService");

            _newBookingCommand = new DelegateCommand(
                async parameter => await OnClickNewBookingAsync());

            _cameraClickCommand = new DelegateCommand(
                async parameter => await OnCameraClickAsync(parameter));

            _viewPhotoClickCommand = new DelegateCommand(
                async parameter => await OnViewPhotoClickAsync(parameter));

            _logoutCommand = new DelegateCommand(
                async parameter => await OnLogoutClickAsync(parameter));

            _ViewBookingCommand = new DelegateCommand(
                async parameter => await OnViewBookingClickAsync(parameter));

            _editBookingCommand = new DelegateCommand(
                async parameter => await OnEditBookingClickAsync(parameter));

            _deleteBookingCommand = new DelegateCommand(
                async parameter => await OnDeleteBookingClickAsync(parameter));

            _bookedByDetailCommand = new DelegateCommand(
                async parameter => await OnBookedByDetailClickAsync(parameter));
        }

        private async Task OnBookedByDetailClickAsync(object parameter)
        {
            if (parameter is BookingDetails bookingDetails)
            {
                Users userDetail = _offlineDatabaseProvider.GetUsersDetail(_loginDetail.UserId);
                UserDetailView userDetailView = new UserDetailView(userDetail);
                await _contentDialogService.ShowDialogAsync(userDetailView);
            }
        }

        private async Task OnClickNewBookingAsync()
        {
            _isViewBooking = false;
            _isEditBooking = false;

            await OnClickBookingAsync(null, _isViewBooking, _isEditBooking);

            if (_bookingDetail != null)
            {
                await _messageDialogService.ShowMessageDialogAsync(
                    ApplicationConstants.TakeAPhoto,
                    ApplicationConstants.GoForPhoto);

                await OnCameraClickAsync(_bookingDetail);
                bool isEmailSent = await SendingEmailAsync(_bookingDetail);

                _bookingDetail.IsEmailSent = isEmailSent;
                _bookingDataProvider.AddOrUpdateBooking(_bookingDetail);

                await _messageDialogService.ShowMessageDialogAsync(
                    ApplicationConstants.Congrats,
                    ApplicationConstants.BookingSuccess.FormatString(
                        _bookingDetail.BookingDate));

                ReloadBooking();
            }
        }

        private async Task OnDeleteBookingClickAsync(object parameter)
        {
            _isViewBooking = false;
            _isEditBooking = false;

            if (parameter is BookingDetails bookingDetails)
            {
                List<IUICommand> commands = new List<IUICommand>
                {
                    new UICommand(
                        label: ApplicationConstants.Yes,
                        action: (uiCommand) => DeleteBooking(bookingDetails)),

                    new UICommand(
                    label: ApplicationConstants.No,
                    action: null)
                };

                await _messageDialogService.ShowMessageDialogAsync(
                    ApplicationConstants.Warning,
                    ApplicationConstants.DeleteWarning,
                    commands);
            }
        }

        private void DeleteBooking(BookingDetails bookingDetails)
        {
            _bookingDataProvider.DeleteBooking(bookingDetails);
            _bookingDetailList.Remove(bookingDetails);

            OnPropertyChanged(() => BookingDetailList);
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

        private async Task<bool> SendingEmailAsync(BookingDetails bookingDetail)
        {
            bool isEmailSent = false;
            App.ShowLoadingIndicator(ApplicationConstants.SendingEmail);
            isEmailSent = await SendBookingMailAsync(bookingDetail);
            App.HideLoadingIndicator();

            return isEmailSent;
        }

        private async Task OnViewBookingClickAsync(object parameter)
        {
            _isViewBooking = true;
            _isEditBooking = false;

            if (parameter is BookingDetails bookingDetail)
                await OnClickBookingAsync(bookingDetail, _isViewBooking, _isEditBooking);
        }

        private async Task OnLogoutClickAsync(object parameter)
        {
            List<IUICommand> commands = new List<IUICommand>
            {
                new UICommand(
                    label: ApplicationConstants.Logout,
                    action: (uiCommand) => LogoutContinue()),

                new UICommand(
                    label: ApplicationConstants.Cancel,
                    action: null)
            };

            await _messageDialogService.ShowMessageDialogAsync(
                ApplicationConstants.Warning,
                ApplicationConstants.LogoutWarning,
                commands);
        }

        private void LogoutContinue()
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
                IsEditBooking = IsEditBooking,
                MasterBookingList = _masterBookingDetailList
            };

            if (bookingDetail != null)
                bookingParameter.BookingDetails = bookingDetail;

            NewBooking newBooking = new NewBooking(bookingParameter);
            newBooking.BookingEvent +=
               async (sender, eventArgs) => await OnBookingCompletedAsync(sender, eventArgs);

            await _contentDialogService.ShowDialogAsync(newBooking);
        }

        private async Task OnBookingCompletedAsync(object sender, AddBookingEventArgs eventArgs)
        {
            if (!_isViewBooking && eventArgs != null)
            {
                _bookingDetail = eventArgs.BookingDetails;

                if (_bookingDetail != null)
                {
                    _bookingDetail.UserId = _loginDetail.UserId;

                    if (!_isEditBooking)
                        _bookingDetail.CreationDate = DateTime.Now.ConvertToString(
                            ApplicationConstants.DatabaseDateTimeFormat);

                    if (_isEditBooking)
                    {
                        bool isEmailSent = await SendingEmailAsync(_bookingDetail);
                        _bookingDetail.IsEmailSent = isEmailSent;
                        _bookingDataProvider.AddOrUpdateBooking(_bookingDetail);
                    }

                    if (_bookingDetailList == null)
                        _bookingDetailList = new ObservableCollection<BookingDetails>();

                    _bookingDetailList.Add(_bookingDetail);
                    _masterBookingDetailList.Add(_bookingDetail);

                    OnPropertyChanged(() => BookingDetailList);
                }
            }
        }

        private async Task<bool> SendBookingMailAsync(BookingDetails bookingDetail)
        {
            if (bookingDetail != null)
            {
                EmailParameter emailParameter = new EmailParameter
                {
                    BookingDetails = bookingDetail,
                    EmailAddress = bookingDetail.EmailId,
                    UserName = bookingDetail.CustomerName
                };

                return await EmailUtility.SendEmailsAsync(emailParameter);
            }

            return false;
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
            _masterBookingDetailList = new ObservableCollection<BookingDetails>(
                _bookingDataProvider.GetAllBookings());

            if (_masterBookingDetailList != null)
            {
                _bookingDetailList = new ObservableCollection<BookingDetails>();
                foreach (BookingDetails booking in _masterBookingDetailList)
                {
                    if (booking.UserId == _loginDetail.UserId)
                        _bookingDetailList.Add(booking);
                }
            }

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

        public ICommand BookedByDetailCommand => _bookedByDetailCommand;

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
