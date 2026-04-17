using BookingBanquetDV.Source.Recover;
using BookingBanquetDV.Source.Register;
using BookingBanquetDV.Source.Welcome;
using BookingBanquetDV.Source.Welcome.Controls;
using BookingBanquetDV.Source.Welcome.PrintOut;
using ViewModelsAndValidation;
using ViewModelsAndValidation.ViewModels;

namespace BookingBanquetDV
{
    public static class ViewModelLocator
    {
        public static IMainViewModel MainViewModel => DIContainer.GetInstance<MainViewModel>();
        public static ISignupViewModel SignupViewModel => DIContainer.GetInstance<SignupViewModel>();
        public static IRecoverUserViewModel RecoverUserViewModel => DIContainer.GetInstance<RecoverUserViewModel>();
        public static IWelcomeViewModel WelcomeViewModel => DIContainer.GetInstance<WelcomeViewModel>();
        public static INewBookingViewModel NewBookingViewModel => DIContainer.GetInstance<NewBookingViewModel>();
        public static IUserDetailViewModel UserDetailViewModel => DIContainer.GetInstance<UserDetailViewModel>();
        public static IAllProfilePhotoViewModel AllProfilePhotoViewModel => DIContainer.GetInstance<AllProfilePhotoViewModel>();
        public static IViewBookingViewModel ViewBookingViewModel => DIContainer.GetInstance<ViewBookingViewModel>();
        public static IPrintLoadingViewModel PrintLoadingViewModel => DIContainer.GetInstance<PrintLoadingViewModel>();
        public static IBanquetConfigViewModel BanquetConfigViewModel => DIContainer.GetInstance<BanquetConfigViewModel>();
    }
}
