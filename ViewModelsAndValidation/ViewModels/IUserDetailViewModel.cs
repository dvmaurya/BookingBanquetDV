using CommonServices.Model;
using System.Windows.Input;
using Windows.UI.Xaml.Media.Imaging;

namespace ViewModelsAndValidation.ViewModels
{
    public interface IUserDetailViewModel
    {
        Users Users { get; set; }
        ICommand OnDialogLoadCommand { get; }
        BitmapImage ImageSource { get; set; }
    }
}
