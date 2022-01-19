using BookingBanquetDV.Source.Camera;
using BookingBanquetDV.Source.Recover;
using BookingBanquetDV.Source.Welcome.Controls;
using BookingBanquetDV.Source.Welcome.PrintOut;
using BookingBanquetDV.Utility;
using BookingServices.Interfaces;
using CommonServices.Analytcis;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace BookingBanquetDV.Source.Welcome
{
    public class WelcomeViewModel : ValidationViewModelBase, IWelcomeViewModel
    {
        private LoginDetail _loginDetail;
        private BookingDetails _bookingDetail;
        private IReadOnlyCollection<BookingDetails> _masterBookingDetailList;
        private ObservableCollection<BookingDetails> _bookingDetailList;

        private readonly ICommand _newBookingCommand;
        private readonly ICommand _sendUserEmailCommand;
        private ICameraViewModel _cameraViewModel;
        private UsersPersonalDetail _usersPersonalDetail;
        private PhotoDetails _photoDetail;
        private BanquetDetail _banquetDetail;
        private readonly ICommand _banquetConfigCommand;
        private readonly ICommand _printOutCommand;
        private readonly ICommand _sendEmailCommand;
        private readonly ICommand _changePasswordCommand;
        private readonly ICommand _editProfileCommand;
        private readonly ICommand _viewProfileCommand;
        private readonly ICommand _cameraClickCommand;
        private readonly ICommand _viewPhotoClickCommand;
        private readonly ICommand _logoutCommand;
        private readonly ICommand _ViewBookingCommand;
        private readonly ICommand _editBookingCommand;
        private readonly ICommand _deleteBookingCommand;
        private readonly IBookingDataProvider _bookingDataProvider;
        private readonly INavigationService _navigationService;
        private readonly IMessageDialogService _messageDialogService;
        private readonly IUsersPersonalDetailDataProvider _usersPersonalDetailDataProvider;
        private readonly IContentDialogService _contentDialogService;
        private readonly IPhotoDetailDataProvider _photoDetailDataProvider;
        private readonly ICustomerPhotoDataProvider _customerPhotoDataProvider;
        private readonly IBanquetDataProvider _banquetDataProvider;
        private readonly IAnalyticsManager _analyticsManager;

        public WelcomeViewModel(
            ICameraViewModel cameraViewModel,
            IBookingDataProvider bookingDataProvider,
            INavigationService navigationService,
            IMessageDialogService messageDialogService,
            IUsersPersonalDetailDataProvider usersPersonalDetailDataProvider,
            IContentDialogService contentDialogService,
            IPhotoDetailDataProvider photoDetailDataProvider,
            ICustomerPhotoDataProvider customerPhotoDataProvider,
            IBanquetDataProvider banquetDataProvider,
            IAnalyticsManager analyticsManager)
        {
            _cameraViewModel = cameraViewModel ?? throw new ArgumentNullException("cameraViewModel");
            _bookingDataProvider = bookingDataProvider ?? throw new ArgumentNullException("bookingDataProvider");
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");
            _messageDialogService = messageDialogService ?? throw new ArgumentNullException("messageDialogService");
            _usersPersonalDetailDataProvider = usersPersonalDetailDataProvider ?? throw new ArgumentNullException("usersPersonalDetailDataProvider");
            _contentDialogService = contentDialogService ?? throw new ArgumentNullException("contentDialogService");
            _photoDetailDataProvider = photoDetailDataProvider ?? throw new ArgumentNullException("photoDetailDataProvider");
            _customerPhotoDataProvider = customerPhotoDataProvider ?? throw new ArgumentNullException("customerPhotoDataProvider");
            _banquetDataProvider = banquetDataProvider ?? throw new ArgumentNullException("banquetDataProvider");
            _analyticsManager = analyticsManager ?? throw new ArgumentNullException("analyticsManager");

            _newBookingCommand = new DelegateCommand(
                parameter => OnClickNewBooking());

            _cameraClickCommand = new DelegateCommand(
                async parameter => await OnCameraClickAsync(parameter));

            _viewPhotoClickCommand = new DelegateCommand(
                async parameter => await OnViewPhotoClickAsync(parameter));

            _logoutCommand = new DelegateCommand(
                async parameter => await OnLogoutClickAsync(parameter));

            _ViewBookingCommand = new DelegateCommand(
                parameter => OnViewBookingClick(parameter));

            _editBookingCommand = new DelegateCommand(
                parameter => OnEditBookingClick(parameter));

            _deleteBookingCommand = new DelegateCommand(
                async parameter => await OnDeleteBookingClickAsync(parameter));

            _viewProfileCommand = new DelegateCommand(
                async parameter => await OnViewProfileClickAsync(parameter));

            _editProfileCommand = new DelegateCommand(
                parameter => OnEditProfileClick());

            _changePasswordCommand = new DelegateCommand(
                parameter => OnChangePassword());

            _sendEmailCommand = new DelegateCommand(
                async parameter => await OnSendEmailAsync(parameter));

            _sendUserEmailCommand = new DelegateCommand(
                async parameter => await OnSendUserEmailAsync());

            _printOutCommand = new DelegateCommand(
                parameter => OnPrintOutAsync(parameter));

            _banquetConfigCommand = new DelegateCommand(
                parameter => OnBanquetConfigAsync());
        }

        private void OnBanquetConfigAsync()
        {
            BanquetConfigDialog banquetConfig = new BanquetConfigDialog(_loginDetail, _banquetDetail);
            _contentDialogService.ShowDialogAsync(banquetConfig);
        }

        private void OnPrintOutAsync(object parameter)
        {
            if (parameter is BookingDetails bookingDetails)
            {
                CustomerPhotoDetail customerPhoto =
                    _customerPhotoDataProvider.GetCustomerPhotoDetail(bookingDetails.Id);

                NavigationParameter navigationParameter = new NavigationParameter
                {
                    BookingDetails = bookingDetails,
                    LoginDetail = _loginDetail
                };

                if (customerPhoto != null)
                    navigationParameter.CustomerPhoto = customerPhoto;

                _navigationService.Navigate(typeof(PrintLoading), navigationParameter);
            }
        }

        private async Task OnSendEmailAsync(object parameter)
        {
            if (parameter is BookingDetails bookingDetails)
            {
                CustomerPhotoDetail customerPhoto =
                    _customerPhotoDataProvider.GetCustomerPhotoDetail(bookingDetails.Id);

                App.ShowLoadingIndicator(ApplicationConstants.SendingEmail);
                bool isEmailSent = await SendBookingMailAsync(bookingDetails, customerPhoto);
                App.HideLoadingIndicator();

                bookingDetails.IsEmailSent = isEmailSent;
                _bookingDataProvider.AddOrUpdateBooking(bookingDetails);

                string emailMessage = string.Empty;

                emailMessage = isEmailSent ?
                    ApplicationConstants.EmailSent.FormatString(bookingDetails.EmailId) :
                    ApplicationConstants.SomethingWentWrong;

                await _messageDialogService.ShowMessageDialogAsync(
                        string.Empty,
                        emailMessage);
            }
        }

        private async Task OnSendUserEmailAsync()
        {
            bool isEmailSent = false;
            App.ShowLoadingIndicator(ApplicationConstants.SendingEmail);

            if (_usersPersonalDetail != null)
            {
                EmailParameter emailParameter = new EmailParameter
                {
                    EmailAddress = _usersPersonalDetail.EmailId,
                    UserName = _usersPersonalDetail.FullName,
                    UsersPersonalDetail = _usersPersonalDetail,
                    PhotoDetails = _photoDetail
                };

                isEmailSent = await EmailUtility.SendEmailsAsync(emailParameter, _analyticsManager);
            }

            App.HideLoadingIndicator();

            string emailMessage = isEmailSent ?
                ApplicationConstants.EmailSent.FormatString(_usersPersonalDetail.EmailId) :
                ApplicationConstants.SomethingWentWrong;

            await _messageDialogService.ShowMessageDialogAsync(
                string.Empty,
                emailMessage);
        }

        private async Task<bool> SendBookingMailAsync(BookingDetails bookingDetail, CustomerPhotoDetail customerPhoto)
        {
            if (bookingDetail != null)
            {
                EmailParameter emailParameter = new EmailParameter
                {
                    BookingDetails = bookingDetail,
                    EmailAddress = bookingDetail.EmailId,
                    UserName = bookingDetail.CustomerName,
                    CustomerPhoto = customerPhoto
                };

                return await EmailUtility.SendEmailsAsync(emailParameter, _analyticsManager);
            }

            return false;
        }

        private void OnChangePassword()
        {
            App.ShowLoadingIndicator(ApplicationConstants.Loading);
            _navigationService.Navigate(typeof(RecoverUser), _loginDetail);
            App.HideLoadingIndicator();
        }

        private void OnEditProfileClick()
        {
            App.ShowLoadingIndicator(ApplicationConstants.Loading);
            if (_usersPersonalDetail != null)
                _navigationService.Navigate(typeof(EditUserDetailPage), _usersPersonalDetail);
            App.HideLoadingIndicator();
        }

        private async Task OnViewProfileClickAsync(object parameter)
        {
            UsersPersonalDetail userDetailViewParam = null;

            if (parameter is BookingDetails bookingDetails)
            {
                if (!bookingDetails.IsCurrentUserBooking)
                {
                    userDetailViewParam = _usersPersonalDetailDataProvider.GetUsersPersonalDetail(bookingDetails.UserId);
                    PhotoDetails photoDetail = _photoDetailDataProvider.GetActivePhotoDetail(userDetailViewParam.EmailId);
                    userDetailViewParam.ImageSource = await SetUserPhoto(photoDetail.LocalPhotoName);
                }
                else if (_usersPersonalDetail != null)
                {
                    userDetailViewParam = _usersPersonalDetail;
                }
            }
            else if (_usersPersonalDetail != null)
            {
                userDetailViewParam = _usersPersonalDetail;
            }

            if (userDetailViewParam != null)
            {
                _analyticsManager.TraceScreenView(
                    $"{AnalyticConstants.User} " +
                    $"{AnalyticConstants.Profile} " +
                    $"{AnalyticConstants.Screen} ");

                ViewUserDetail userDetailView = new ViewUserDetail(userDetailViewParam);
                await _contentDialogService.ShowDialogAsync(userDetailView);
            }
        }

        private void OnClickNewBooking()
        {
            BookingParameter bookingParameter = new BookingParameter
            {
                LoginDetail = _loginDetail
            };

            _navigationService.Navigate(typeof(NewBookingDetail), bookingParameter);
        }

        private async Task OnDeleteBookingClickAsync(object parameter)
        {
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
            if (bookingDetails != null)
            {
                _bookingDataProvider.DeleteBooking(bookingDetails);
                _bookingDetailList.Remove(bookingDetails);

                CustomerPhotoDetail deletePhoto =
                    _customerPhotoDataProvider.GetCustomerPhotoDetail(bookingDetails.Id);

                if (deletePhoto != null)
                    _customerPhotoDataProvider.DeleteCustomerPhotoAsync(deletePhoto);

                OnPropertyChanged(() => BookingDetailList);
            }
        }

        private void OnEditBookingClick(object parameter)
        {
            if (parameter is BookingDetails bookingDetail)
                OnClickBooking(bookingDetail);
        }

        private void OnViewBookingClick(object parameter)
        {
            if (parameter is BookingDetails bookingDetail)
            {
                CustomerPhotoDetail customerPhoto =
                    _customerPhotoDataProvider.GetCustomerPhotoDetail(bookingDetail.Id);

                ViewBookingContent viewBooking = new ViewBookingContent(bookingDetail, customerPhoto);
                _contentDialogService.ShowDialogAsync(viewBooking);
            }
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

            _analyticsManager.TraceEvents(
                analyticsEvent: $"{AnalyticConstants.Application} ",
                action: $"{AnalyticConstants.Logout} ",
                label: $"{AnalyticConstants.User} " +
                        $"{AnalyticConstants.Clicked} " +
                        $"{AnalyticConstants.Logout} ");

            _navigationService.Navigate(typeof(MainPage), _loginDetail);
        }

        private async Task OnViewPhotoClickAsync(object parameter)
        {
            if (parameter is BookingDetails bookingDetail)
            {
                _analyticsManager.TraceEvents(
                    analyticsEvent: $"{AnalyticConstants.Application} ",
                    action: $"{AnalyticConstants.Customer} " +
                            $"{AnalyticConstants.Photo} ",
                    label: $"{AnalyticConstants.Customer} " +
                            $"{AnalyticConstants.Photo} " +
                            $"{AnalyticConstants.Open} ");

                CustomerPhotoDetail photoDetails = new CustomerPhotoDetail();
                StorageFolder storageFolder = await ApplicationData.Current.LocalFolder.
                    GetFolderAsync(ApplicationConstants.CustomerPhoto);

                photoDetails = _customerPhotoDataProvider.GetCustomerPhotoDetail(bookingDetail.Id);

                if (await storageFolder.TryGetItemAsync(photoDetails.PhotoName) is StorageFile file)
                {
                    photoDetails.ImageSource = await PhotoUtility.GetDecrtypedPhotoAsync(file, ApplicationConstants.CustomerPhoto);
                    photoDetails.StorageFile = file;

                    PreviewPhoto newBooking = new PreviewPhoto(photoDetails);
                    await _contentDialogService.ShowDialogAsync(newBooking);
                }
            }
        }

        private async Task OnCameraClickAsync(object parameter)
        {
            if (parameter is BookingDetails bookingDetail)
            {
                _analyticsManager.TraceEvents(
                    analyticsEvent: $"{AnalyticConstants.Application} ",
                    action: $"{AnalyticConstants.Open} " +
                            $"{AnalyticConstants.Camera}",
                    label: $"{AnalyticConstants.Open} " +
                            $"{AnalyticConstants.Camera} " +
                            $"for {AnalyticConstants.Add} " +
                            $"{AnalyticConstants.Customer} " +
                            $"{AnalyticConstants.Photo} ");

                CustomerPhotoDetail customerPhoto = await _cameraViewModel.OnClickCustomerPhotoAsync(
                    ApplicationConstants.CustomerPhoto);

                if (customerPhoto != null)
                {
                    customerPhoto.BookingId = bookingDetail.Id;
                    customerPhoto.EmailId = bookingDetail.EmailId;
                    bookingDetail.CanShowPhoto = true;
                    _customerPhotoDataProvider.AddOrUpdatePhotoDetail(customerPhoto);
                }
            }
        }

        private void OnClickBooking(BookingDetails bookingDetail)
        {
            BookingParameter bookingParameter = new BookingParameter
            {
                BookingDetails = bookingDetail,
                LoginDetail = _loginDetail
            };

            _navigationService.Navigate(typeof(NewBookingDetail), bookingParameter);
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode navigationMode)
        {
            await base.OnNavigatedToAsync(parameter, navigationMode);

            if (parameter is LoginDetail loginDetail)
            {
                _loginDetail = loginDetail;
                await LoadAllBookingDetailAsync();

                _usersPersonalDetail = _usersPersonalDetailDataProvider.GetUsersPersonalDetail(_loginDetail.MobileNo);

                if (_usersPersonalDetail != null)
                {
                    _loginDetail.UserName = _usersPersonalDetail.FullName;
                    _photoDetail = _photoDetailDataProvider.GetActivePhotoDetail(_usersPersonalDetail.EmailId);
                    _usersPersonalDetail.ImageSource = await SetUserPhoto(_photoDetail.LocalPhotoName);
                }

                OnPropertyChanged(() => UsersPersonalDetail);

                _banquetDetail = _banquetDataProvider.GetBanquetDetail();
            }
        }

        private async Task<BitmapImage> SetUserPhoto(string photoName)
        {
            BitmapImage imageSource = null;
            if (photoName != null)
            {
                StorageFolder storageFolder = await ApplicationData.Current.LocalFolder.
                    GetFolderAsync(ApplicationConstants.Document);

                if (await storageFolder.TryGetItemAsync(photoName) is StorageFile file)
                    imageSource = await PhotoUtility.GetDecrtypedPhotoAsync(file, ApplicationConstants.Document);
            }

            return imageSource;
        }

        private async Task LoadAllBookingDetailAsync()
        {
            _masterBookingDetailList = new ObservableCollection<BookingDetails>(
                _bookingDataProvider.GetAllBookings());

            if (_masterBookingDetailList != null)
            {
                _bookingDetailList = new ObservableCollection<BookingDetails>();
                foreach (BookingDetails booking in _masterBookingDetailList)
                {
                    _bookingDetailList.Add(booking);
                    CustomerPhotoDetail photo = _customerPhotoDataProvider.GetCustomerPhotoDetail(booking.Id);

                    if (photo != null && !string.IsNullOrEmpty(photo.PhotoName))
                        booking.CanShowPhoto = true;

                    if (booking.UserId == _loginDetail.MobileNo || booking.UserId == _loginDetail.EmailId)
                        booking.IsCurrentUserBooking = true;

                    PhotoDetails userPhoto = _photoDetailDataProvider.GetActivePhotoDetail(booking.UserId);

                    if (userPhoto != null)
                    {
                        StorageFolder storageFolder = await ApplicationData.Current.LocalFolder.
                            GetFolderAsync(ApplicationConstants.Document);

                        if (await storageFolder.TryGetItemAsync(userPhoto.LocalPhotoName) is StorageFile file)
                            booking.UserPhoto = await PhotoUtility.GetDecrtypedPhotoAsync(file, ApplicationConstants.Document);
                    }
                }
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

        public ICommand ViewProfileCommand => _viewProfileCommand;

        public ICommand EditProfileCommand => _editProfileCommand;

        public UsersPersonalDetail UsersPersonalDetail
        {
            get => _usersPersonalDetail;
            set => _usersPersonalDetail = value;
        }

        public ICommand ChangePasswordCommand => _changePasswordCommand;

        public ICommand SendEmailCommand => _sendEmailCommand;

        public ICommand PrintOutCommand => _printOutCommand;

        public ICommand SendUserEmailCommand => _sendUserEmailCommand;

        public ICommand BanquetConfigCommand => _banquetConfigCommand;

        public BanquetDetail BanquetDetail
        {
            get => _banquetDetail;
            set => _banquetDetail = value;
        }
    }
}
