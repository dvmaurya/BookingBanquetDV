using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModelsAndValidation.ViewModels
{
    public interface ILoginViewModel
    {
        ICommand LoginCommand { get; }
        Task<bool> NavigateToWelcomePageAsync(object parameter);
    }
}
