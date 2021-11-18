using CommonServices.Analytcis;
using System;
using System.Linq;
using System.Threading.Tasks;
using ViewModelsAndValidation;
using ViewModelsAndValidation.Services;
using ViewModelsAndValidation.Views;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace BookingBanquetDV.Navigation
{
    public class NavigationService : INavigationService
    {
        private readonly Frame _frame;
        private Task _navigatingFromTask;
        private bool _isDoubleClicked;
        private IAnalyticsManager _analyticsManager;

        public NavigationService(
            Frame frame)
        {
            _frame = frame ?? throw new ArgumentNullException(nameof(frame));

            _frame.Navigated += OnFrameNavigatedTo;
            _frame.Navigating += OnFrameNavigatingFrom;

            _analyticsManager = DIContainer.GetInstance<IAnalyticsManager>();
        }

        public async Task OnBackRequestedAsync(object sender, BackRequestedEventArgs backRequestedEventArgs)
        {
            if (!_isDoubleClicked)
            {
                _isDoubleClicked = true;

                if (_frame == null)
                    return;

                if (CurrentView != null)
                {
                    if (CurrentView.ViewModel is CustomGoBackBase customGobackViewmodel)
                    {
                        backRequestedEventArgs.Handled = true;

                        if (!await customGobackViewmodel.CanExecuteBackRequestedEventAsync().ConfigureAwait(true))
                        {
                            customGobackViewmodel.HandleGoBack();
                            _isDoubleClicked = false;
                            return;
                        }
                    }
                }

                if (_frame.CanGoBack)
                {
                    _frame.GoBack();
                    backRequestedEventArgs.Handled = true;
                }

                _isDoubleClicked = false;
            }
        }

        public IView CurrentView
        {
            get { return _frame.Content as IView; }
        }

        public string CurrentFrame => _frame.Name;

        public void GoBack()
        {
            if (_frame.CanGoBack)
                _frame.GoBack();
        }

        public void Navigate(Type type)
        {
            if (type != null)
            {
                TraceScreenVisit(type);
                _frame.Navigate(type);
            }
        }

        public void Navigate(Type type, object parameter)
        {
            if (type != null &&
                parameter != null)
            {
                TraceScreenVisit(type);
                _frame.Navigate(type, parameter);
            }
        }

        private void TraceScreenVisit(Type type)
        {
            string screenName = type.ToString().Split('.').Last();

            _analyticsManager.TraceScreenView(screenName);
        }

        public async Task SuspendingAsync()
        {
            await NavigateFromCurrentViewModelAsync(true);
        }

        private async Task NavigateToCurrentViewModelAsync(NavigationMode navigationMode, object parameter)
        {
            if (!(_frame.Content is IView newView))
                return;

            if (!_frame.CanGoBack)
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;

            ViewModelBase newViewModel = newView.ViewModel;

            if (newViewModel != null)
                await newViewModel.OnNavigatedToAsync(parameter, navigationMode);
        }

        private async Task NavigateFromCurrentViewModelAsync(bool suspending)
        {
            if (!(_frame.Content is IView departingView))
                return;

            ViewModelBase departingViewModel = departingView.ViewModel;

            if (departingViewModel != null)
            {
                _navigatingFromTask = departingViewModel.OnNavigatingFromAsync(suspending);
                await _navigatingFromTask;
                _navigatingFromTask = null;
            }
        }

        private async void OnFrameNavigatingFrom(object sender, NavigatingCancelEventArgs e)
        {
            await NavigateFromCurrentViewModelAsync(false);
        }

        private async void OnFrameNavigatedTo(object sender, NavigationEventArgs e)
        {
            if (_navigatingFromTask != null)
                await Task.WhenAll(_navigatingFromTask);

            await NavigateToCurrentViewModelAsync(e.NavigationMode, e.Parameter);
        }

        public void ClearNavigationStack()
        {
            _frame.BackStack.Clear();
        }
    }
}
