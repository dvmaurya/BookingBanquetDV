using BookingServices.Interfaces;
using CommonServices.ConstantData;
using CommonServices.Extensions;
using CommonServices.FileReaders;
using CommonServices.Logger;
using CommonServices.Model.City;
using CommonServices.Utilities;
using System;
using System.Collections.Generic;
using ViewModelsAndValidation;
using ViewModelsAndValidation.Services;
using ViewModelsAndValidation.Views;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DBModel = CommonServices.Model.DbModel;

namespace BookingBanquetDV
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        /// 

        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            this.Resuming += OnResuming;
            this.UnhandledException += App_UnhandledException;
        }

        public static ILogger Logger { get { return FileLogger.Logger; } }

        private void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {

        }

        private void OnResuming(object sender, object e)
        {

        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            try
            {
                if (args.PreviousExecutionState == ApplicationExecutionState.Running)
                {
                    if (Window.Current != null &&
                        Window.Current.Content != null)
                        Window.Current.Activate();

                    return;
                }

                if (args.Arguments != ApplicationConstants.ToastNotification)
                {
                    ShowLoadingIndicator(ApplicationConstants.Loading);
                    FileLogger.Logger.Info(
                        ApplicationConstants.AppVersion +
                        " : " +
                        Utilities.AppVersion);

                    DisplayInformation.AutoRotationPreferences = DisplayOrientations.Landscape | DisplayOrientations.LandscapeFlipped;

                    Initiator.LoadTypes();

                    //_exceptionHandlerFactory = DIContainer.GetInstance<IExceptionHandlerFactory>();

                    SystemNavigationManager.GetForCurrentView().BackRequested -= App_BackRequested;
                    SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;

                    DIContainer.GetInstance<IOfflineDatabaseProvider>().CreateOfflineDatabase();
                    GetConutryStateAndCityData();
                    
                    //await CheckForAppUpdates();
                    ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

                    var currentView = ApplicationView.GetForCurrentView();
                    currentView.Title = string.Empty;

                    ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
                    titleBar.BackgroundColor = ((SolidColorBrush)App.Current.Resources["TitleBarBackGround"]).Color;
                    titleBar.ForegroundColor = Colors.White;
                    titleBar.ButtonBackgroundColor = ((SolidColorBrush)App.Current.Resources["TitleBarBackGround"]).Color;
                    titleBar.ButtonForegroundColor = Colors.White;
                    RegisterAppLoginPage();
                    //await InitializeCertificateProcessAsync();
                    HideLoadingIndicator();
                }
            }
            catch (Exception ex)
            {
                FileLogger.Logger.Info("Exception: ".ConcatString(ex.Message));
            }
        }

        private void GetConutryStateAndCityData()
        {
            List<Country> country = CountryStateAndCity.GetStateAndCity();
            DIContainer.GetInstance<IOfflineDatabaseProvider>().StoreCountryData(country);
        }

        private void RegisterAppLoginPage()
        {
            if (!(Window.Current.Content is MainPage rootFrame))
            {
                rootFrame = new MainPage();
                Window.Current.Content = rootFrame;
                DIContainer.GetInstance<INavigationService>().Navigate(typeof(MainPage));
                Window.Current.Activate();
            }
        }

        public static void ShowLoadingIndicator(string contentText)
        {
            if (Window.Current.Content is IAppFrame frame)
                frame.ShowLoadingIndicator(contentText);
        }

        public static void HideLoadingIndicator()
        {
            if (Window.Current.Content is IAppFrame frame)
                frame.HideLoadingIndicator();
        }

        private async void App_BackRequested(object sender, BackRequestedEventArgs e)
        {
            await DIContainer.GetInstance<INavigationService>().OnBackRequestedAsync(sender, e).ConfigureAwait(true);
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if(sender != null) { }
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
