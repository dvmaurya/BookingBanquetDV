using BookingBanquetDV.Source.Recover;
using BookingBanquetDV.Source.Register;
using BookingBanquetDV.Source.Welcome;
using BookingBanquetDV.Source.Welcome.Controls;
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
        public static IUserDetailViewModel userDetailViewModel => DIContainer.GetInstance<UserDetailViewModel>();
    }
}
