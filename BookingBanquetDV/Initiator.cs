using BookingBanquetDV.Services;
using BookingBanquetDV.Source.Camera;
using BookingBanquetDV.Source.Login;
using BookingBanquetDV.Source.Recover;
using BookingBanquetDV.Source.Register;
using BookingBanquetDV.Source.Welcome;
using BookingServices.DataProvider;
using BookingServices.Interfaces;
using BookingServices.Repository;
using BookingServices.Repository.Interface;
using BookingServices.SQLite;
using CommonServices.Analytcis;
using CommonServices.Logger;
using System.IO;
using Unity.Injection;
using ViewModelsAndValidation;
using ViewModelsAndValidation.Services;
using ViewModelsAndValidation.ViewModels;
using Windows.Storage;

namespace BookingBanquetDV
{
    public static class Initiator
    {
        public static void LoadTypes()
        {
            RegisterServices();
            RegisterViewModels();
            RegisterRepositories();
        }

        private static void RegisterRepositories()
        {
            DIContainer.RegisterType<IOfflineDatabaseProvider, OfflineDatabaseProvider>();
            DIContainer.RegisterType<IEncryptionProvider, EncryptionProvider>();
            DIContainer.RegisterType<IBookingDataProvider, BookingDataProvider>();

            DIContainer.RegisterType<ILoginRepository, LoginRepository>();
            DIContainer.RegisterType<IUserRepository, UserRepository>();
            DIContainer.RegisterType<IBookingRepository, BookingRepository>();
            DIContainer.RegisterType<IEncryptionProvider, EncryptionProvider>();
            DIContainer.RegisterSingletonType<IDbContext, SQLiteContext>(
                 new InjectionConstructor(
                     Path.Combine(ApplicationData.Current.LocalFolder.Path, "Banquet.sqlite"),
                     "KVyZKGVns7RoudZOSW9Sfwl+GYMYIL7qqvQgbXdIdJASk7eDxwKQXj7wPjhRCwsJfRrYbr+vYWGJBD08MRHxmw==",
                     "OmNpZDpzdXBwbGllcmludm9pY2VzLnBjY0BlbWlzaGVhbHRoLmNvbTpwbGF0Zm9ybToyNTY6ZXhwaXJlOm5ldmVyOnZlcnNpb246MTpobWFjOmM3MjM2NDdkOTE1NjdiMmFiYWEwMmZkNjBkYWJmNjA1YjJmZDYwYWY"));
        }

        private static void RegisterViewModels()
        {
            DIContainer.RegisterType<IViewModel, MainViewModel>();
            DIContainer.RegisterType<ILoginViewModel, LoginViewModel>();
            DIContainer.RegisterType<ISignupViewModel, SignupViewModel>();
            DIContainer.RegisterType<IRecoverUserViewModel, RecoverUserViewModel>();
            DIContainer.RegisterType<IWelcomeViewModel, WelcomeViewModel>();
            DIContainer.RegisterType<INewBookingViewModel, NewBookingViewModel>();
            DIContainer.RegisterType<ICameraViewModel, CameraViewModel>();
        }

        private static void RegisterServices()
        {
            DIContainer.RegisterInstance<ILogger>(App.Logger);
            DIContainer.RegisterSingletonType<IMessageDialogService, MessageDialogService>();
            DIContainer.RegisterInstance<IAnalyticsManager>(AnalyticsManager.Instance);
        }
    }
}
