using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace ViewModelsAndValidation
{
    public interface IViewModel
    {
        [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "1#", Justification = "Needs to set the referenced value, as part of the set constraint")]
        void SetValue<T>(Expression<Func<T>> expression, ref T field, T value);
        ViewModelState ViewModelState { get; }
        void OnViewModelStateChanged();
        Task OnNavigatingFromAsync(bool suspending);
        Task OnNavigatedToAsync(object parameter, NavigationMode navigationMode);
    }
}
