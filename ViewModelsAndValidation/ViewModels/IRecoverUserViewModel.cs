using CommonServices.Model;
using System.Windows.Input;

namespace ViewModelsAndValidation.ViewModels
{
    public interface IRecoverUserViewModel
    {
        string EmailId { get; set; }
        string MobileNo { get; set; }
        string OTP { get; set; }
        string GeneratedOTP { get; set; }
        string Password { get; set; }
        string ConfirmPassword { get; set; }
        string RecoverUserPasswordTitle { get; set; }
        bool IsOtpEnable { get; set; }
        bool SomethingWrongMessage { get; set; }
        bool InvalidCredential { get; set; }
        bool IsRecoverUIVisible { get; set; }
        bool IsNewPasswordVisible { get; set; }
        bool PasswordUpdateFailed { get; set; }
        ICommand RecoverCommand { get; }
        ICommand LoginCommand { get; }
        ICommand GetOTPCommand { get; }
        ICommand ChangePasswordCommand { get; }
        LoginDetail LoginDetail { get; set; }
    }
}
