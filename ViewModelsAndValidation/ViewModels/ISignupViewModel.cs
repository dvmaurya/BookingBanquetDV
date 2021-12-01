using CommonServices.Model.City;
using System.Collections.Generic;
using System.Windows.Input;

namespace ViewModelsAndValidation.ViewModels
{
    public interface ISignupViewModel
    {
        string FullName { get; set; }
        string EmailId { get; set; }
        string MobileNo { get; set; }
        string Password { get; set; }
        string OTP { get; set; }
        string ConfirmPassword { get; set; }
        bool IsMobileExist { get; set; }
        bool IsEmailExist { get; set; }
        bool IsOtpEnable { get; set; }
        bool SomethingWrongMessage { get; set; }
        bool IsResendOTP { get; set; }
        string Address { get; set; }
        Country SelectedCountry { get; set; }
        State SelectedState { get; set; }
        City SelectedCity { get; set; }
        string SelectedPhoneCode { get; set; }
        ICommand OTPCommand { get; }
        ICommand LoginCommand { get; }
        ICommand SignupCommand { get; }
        ICommand ResetCommand { get; }
        string GeneratedOTP { get; }
        IReadOnlyCollection<Country> CountryList { get; set; }
        IReadOnlyCollection<State> StateList { get; set; }
        IReadOnlyCollection<City> CityList { get; set; }
        bool IsCityVisible { get; set; }
        bool IsStateVisible { get; set; }
        ICommand ChangedCountryCommand { get; }
        ICommand ChangedStateCommand { get; }
        IReadOnlyCollection<string> PhoneCodeList { get; set; }
    }
}
