using System.Threading.Tasks;


namespace ViewModelsAndValidation
{
    public abstract class CustomGoBackBase : ValidationViewModelBase
    {
        public abstract Task<bool> CanExecuteBackRequestedEventAsync();

        public virtual void HandleGoBack()
        {

        }
    }
}
