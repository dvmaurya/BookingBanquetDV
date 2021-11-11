using System.Windows.Input;

namespace ViewModelsAndValidation.ViewModels
{
    public interface IRecoverUserViewModel
    {
        string EmailId { get; set; }
        string MobileNo { get; set; }
        string OTP { get; set; }
        string GeneratedOTP { get; set; }
        bool IsRecoverEnable { get; set; }
        ICommand RecoverCommand { get; }
        ICommand LoginCommand { get; }
        ICommand GetOTPCommand { get; }
    }
}
