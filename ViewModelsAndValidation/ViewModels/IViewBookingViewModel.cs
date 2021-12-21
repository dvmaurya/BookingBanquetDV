using CommonServices.Model;
using System.Windows.Input;

namespace ViewModelsAndValidation.ViewModels
{
    public interface IViewBookingViewModel
    {
        BookingDetails BookingDetails { get; set; }
        CustomerPhotoDetail CustomerPhoto { get; set; }
        ICommand OnLoadDialogCommand { get; }
    }
}
