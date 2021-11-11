using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace ViewModelsAndValidation.Services
{
    public interface IMessageDialogService
    {
        Task<IUICommand> ShowMessageDialogAsync(string title, string content);
        Task<IUICommand> ShowMessageDialogAsync(string title, string content, IReadOnlyCollection<IUICommand> commands);
    }
}
