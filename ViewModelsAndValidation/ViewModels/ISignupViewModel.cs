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
        SingleSelection SelectedCountry { get; set; }
        SingleSelection SelectedState { get; set; }
        SingleSelection SelectedCity { get; set; }
        ICommand OTPCommand { get; }
        ICommand LoginCommand { get; }
        ICommand SignupCommand { get; }
        ICommand ResetCommand { get; }
        string GeneratedOTP { get; }
        IReadOnlyCollection<SingleSelection> CountryList { get; set; }
        IReadOnlyCollection<SingleSelection> StateList { get; set; }
        IReadOnlyCollection<SingleSelection> CityList { get; set; }
        bool IsCityVisible { get; set; }
        bool IsStateVisible { get; set; }
        ICommand ChangedCountryCommand { get; }
        ICommand ChangedStateCommand { get; }
        ICommand ChangedCityCommand { get; }
        IReadOnlyCollection<string> PhoneCodeList { get; set; }
    }
}
