using BookingBanquetDV.Source.Camera;
using BookingBanquetDV.Source.Recover;
using BookingBanquetDV.Source.Welcome.Controls;
using BookingBanquetDV.Utility;
using BookingServices.Interfaces;
using CommonServices.ConstantData;
using CommonServices.Enums;
using CommonServices.Extensions;
using CommonServices.Model;
using CommonServices.Utilities;
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
        private IReadOnlyCollection<BookingDetails> _masterBookingDetailList;
        private ObservableCollection<BookingDetails> _bookingDetailList;

        private readonly ICommand _newBookingCommand;
        private ICameraViewModel _cameraViewModel;
        private UsersPersonalDetail _usersPersonalDetail;
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

        public WelcomeViewModel(
            ICameraViewModel cameraViewModel,
            IBookingDataProvider bookingDataProvider,
            INavigationService navigationService,
            IMessageDialogService messageDialogService,
            IUsersPersonalDetailDataProvider usersPersonalDetailDataProvider,
            IContentDialogService contentDialogService,
            IPhotoDetailDataProvider photoDetailDataProvider,
            ICustomerPhotoDataProvider customerPhotoDataProvider)
        {
            _cameraViewModel = cameraViewModel ?? throw new ArgumentNullException("cameraViewModel");
            _bookingDataProvider = bookingDataProvider ?? throw new ArgumentNullException("bookingDataProvider");
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");
            _messageDialogService = messageDialogService ?? throw new ArgumentNullException("messageDialogService");
            _usersPersonalDetailDataProvider = usersPersonalDetailDataProvider ?? throw new ArgumentNullException("usersPersonalDetailDataProvider");
            _contentDialogService = contentDialogService ?? throw new ArgumentNullException("contentDialogService");
            _photoDetailDataProvider = photoDetailDataProvider ?? throw new ArgumentNullException("photoDetailDataProvider");
            _customerPhotoDataProvider = customerPhotoDataProvider ?? throw new ArgumentNullException("customerPhotoDataProvider");

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
                async parameter => await OnViewProfileClickAsync());

            _editProfileCommand = new DelegateCommand(
                parameter => OnEditProfileClick());

            _changePasswordCommand = new DelegateCommand(
                parameter => OnChangePassword());
        }

        private void OnChangePassword()
        {
            _navigationService.Navigate(typeof(RecoverUser), _loginDetail);
        }

        private void OnEditProfileClick()
        {
            if (_usersPersonalDetail != null)
                _navigationService.Navigate(typeof(EditUserDetailPage), _usersPersonalDetail);
        }

        private void OnSaveUserDetail(object sender, UserDetailEventArgs eventArgs)
        {
            if (eventArgs != null &&
                eventArgs.PreviewDialogAction == PreviewDialogAction.Submit)
            {
                _usersPersonalDetailDataProvider.AddOrUpdateUsersPersonalDetail(eventArgs.UsersPersonalDetail);
            }
        }

        private async Task OnViewProfileClickAsync()
        {
            if (_usersPersonalDetail != null)
            {
                ViewUserDetail userDetailView = new ViewUserDetail(_usersPersonalDetail);
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
            _bookingDataProvider.DeleteBooking(bookingDetails);
            _bookingDetailList.Remove(bookingDetails);

            OnPropertyChanged(() => BookingDetailList);
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

                ViewBooking viewBooking = new ViewBooking(bookingDetail, customerPhoto);
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

            _navigationService.Navigate(typeof(MainPage));
        }

        private async Task OnViewPhotoClickAsync(object parameter)
        {
            if (parameter is BookingDetails bookingDetail)
            {
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
                LoadAllBookingDetail();

                _usersPersonalDetail = _usersPersonalDetailDataProvider.GetUsersPersonalDetail(_loginDetail.UserId);

                if (_usersPersonalDetail != null)
                {
                    _loginDetail.UserName = _usersPersonalDetail.FullName;
                    PhotoDetails photoDetail = _photoDetailDataProvider.GetActivePhotoDetail(_usersPersonalDetail.EmailId);

                    if (photoDetail != null)
                    {
                        StorageFolder storageFolder = await ApplicationData.Current.LocalFolder.
                            GetFolderAsync(ApplicationConstants.Document);

                        if (await storageFolder.TryGetItemAsync(photoDetail.LocalPhotoName) is StorageFile file)
                            _usersPersonalDetail.ImageSource = await PhotoUtility.GetDecrtypedPhotoAsync(file, ApplicationConstants.Document);
                    }

                    OnPropertyChanged(() => UsersPersonalDetail);
                }
            }
        }

        private void LoadAllBookingDetail()
        {
            _masterBookingDetailList = new ObservableCollection<BookingDetails>(
                _bookingDataProvider.GetAllBookings());

            if (_masterBookingDetailList != null)
            {
                _bookingDetailList = new ObservableCollection<BookingDetails>();
                foreach (BookingDetails booking in _masterBookingDetailList)
                {
                    if (booking.UserId == _loginDetail.UserId || booking.UserId == _loginDetail.EmailId)
                    {
                        _bookingDetailList.Add(booking);
                        CustomerPhotoDetail photo = _customerPhotoDataProvider.GetCustomerPhotoDetail(booking.Id);

                        if (photo != null && !string.IsNullOrEmpty(photo.PhotoName))
                            booking.CanShowPhoto = true;
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
    }
}
