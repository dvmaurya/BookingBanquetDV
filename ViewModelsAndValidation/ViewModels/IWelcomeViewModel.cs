using CommonServices.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ViewModelsAndValidation.ViewModels
{
    public interface IWelcomeViewModel
    {
        ICommand NewBookingCommand { get; }
        BookingDetails BookingDetail { get; set; }
        ICameraViewModel CameraViewModel { get; set; }
        ObservableCollection<BookingDetails> BookingDetailList { get; }
    }
}
