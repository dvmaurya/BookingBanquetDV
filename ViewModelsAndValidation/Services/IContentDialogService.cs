using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace ViewModelsAndValidation.Services
{
    public interface IContentDialogService
    {
        Task ShowDialogAsync(ContentDialog dialog);
    }
}
