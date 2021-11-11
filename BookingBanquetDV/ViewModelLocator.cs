using BookingBanquetDV.Source.Recover;
using BookingBanquetDV.Source.Register;
using ViewModelsAndValidation;
using ViewModelsAndValidation.ViewModels;

namespace BookingBanquetDV
{
    public static class ViewModelLocator
    {
        public static IMainViewModel MainViewModel => DIContainer.GetInstance<MainViewModel>();
        public static ISignupViewModel SignupViewModel => DIContainer.GetInstance<SignupViewModel>();
        public static IRecoverUserViewModel RecoverUserViewModel => DIContainer.GetInstance<RecoverUserViewModel>();
    }
}
