using System;
using System.Threading.Tasks;
using ViewModelsAndValidation.Views;
using Windows.UI.Core;

namespace ViewModelsAndValidation.Services
{
    public interface INavigationService
    {
        void Navigate(Type type);
        void Navigate(Type type, object parameter);
        Task SuspendingAsync();
        void GoBack();
        void ClearNavigationStack();
        IView CurrentView { get; }
        string CurrentFrame { get; }
        Task OnBackRequestedAsync(object sender, BackRequestedEventArgs backRequestedEventArgs);
    }
}
