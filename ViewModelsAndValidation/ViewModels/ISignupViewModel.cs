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
        string ConfirmPassword { get; set; }
        bool IsMobileExist { get; set; }
        bool IsEmailExist { get; set; }
        ICommand SignupCommand { get; }
        ICommand LoginCommand { get; }
    }
}
