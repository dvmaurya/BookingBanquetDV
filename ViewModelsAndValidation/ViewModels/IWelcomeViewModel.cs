using CommonServices.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ViewModelsAndValidation.ViewModels
{
    public interface IWelcomeViewModel
    {
        ICommand NewBookingCommand { get; }
        ICommand CameraClickCommand { get; }
        ICommand ViewPhotoClickCommand { get; }
        ICommand LogoutCommand { get; }
        ICommand ViewBookingCommand { get; }
        ICommand EditBookingCommand { get; }
        ICommand DeleteBookingCommand { get; }
        ICommand BookedByDetailCommand { get; }
        BookingDetails BookingDetail { get; set; }
        ICameraViewModel CameraViewModel { get; set; }
        ObservableCollection<BookingDetails> BookingDetailList { get; }
        bool IsViewBooking { get; set; }
        bool IsEditBooking { get; set; }
    }
}
