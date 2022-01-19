using CommonServices.Model;
using System.Windows.Input;

namespace ViewModelsAndValidation.ViewModels
{
    public interface IPrintLoadingViewModel
    {
        BookingDetails BookingDetails { get; set; }
        CustomerPhotoDetail CustomerPhoto { get; set; }
        LoginDetail LoginDetail { get; set; }
        ICommand CloseCommand { get; }
    }
}
