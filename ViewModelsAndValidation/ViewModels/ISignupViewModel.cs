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
        ICommand OTPCommand { get; }
        ICommand LoginCommand { get; }
        ICommand SignupCommand { get; }
        ICommand ResetCommand { get; }
        string GeneratedOTP { get; }
    }
}
