using CommonServices.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ViewModelsAndValidation.ViewModels
{
    public interface IWelcomeViewModel
    {
        ICommand ChangePasswordCommand { get; }
        ICommand EditProfileCommand { get; }
        ICommand NewBookingCommand { get; }
        ICommand CameraClickCommand { get; }
        ICommand ViewPhotoClickCommand { get; }
        ICommand LogoutCommand { get; }
        ICommand ViewBookingCommand { get; }
        ICommand EditBookingCommand { get; }
        ICommand DeleteBookingCommand { get; }
        ICommand ViewProfileCommand { get; }
        ICommand SendEmailCommand { get; }
        ICommand PrintOutCommand { get; }
        ICommand SendUserEmailCommand { get; }
        ICommand BanquetConfigCommand { get; }
        BookingDetails BookingDetail { get; set; }
        BanquetDetail BanquetDetail { get; set; }
        ICameraViewModel CameraViewModel { get; set; }
        ObservableCollection<BookingDetails> BookingDetailList { get; }
        UsersPersonalDetail UsersPersonalDetail { get; set; }
    }
}
