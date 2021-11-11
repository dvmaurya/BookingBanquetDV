using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace ViewModelsAndValidation.ViewModels
{
    public interface IMainViewModel
    {
        ICommand LoginCommand { get; }
        ICommand SignupCommand { get; }
        ICommand ForgotPasswordCommand { get; }
        string UserId { get; set; }
        string Password { get; set; }
        bool IsCredentialWrong { get; set; }
        bool IsRememberChecked { get; set; }
        void GetPreviousLoginDetails();
    }
}
