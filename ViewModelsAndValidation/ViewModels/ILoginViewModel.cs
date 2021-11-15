using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModelsAndValidation.ViewModels
{
    public interface ILoginViewModel
    {
        Task<bool> NavigateToWelcomePageAsync(object parameter);
    }
}
