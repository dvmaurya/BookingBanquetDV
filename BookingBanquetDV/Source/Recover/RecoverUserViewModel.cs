using BookingBanquetDV.Utility;
using BookingBanquetDV.Validation;
using CommonServices.ConstantData;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewModelsAndValidation;
using ViewModelsAndValidation.Command;
using ViewModelsAndValidation.Services;
using ViewModelsAndValidation.ViewModels;
using Windows.ApplicationModel.Contacts;
using Windows.Storage;

namespace BookingBanquetDV.Source.Recover
{
    public class RecoverUserViewModel : ValidationViewModelBase, IRecoverUserViewModel
    {
        private string _emailId;
        private string _mobileNo;
        private string _oTP;
        private bool _isRecoverEnable;
        private string _generatedOTP;
        private readonly ICommand _getOTPCommand;
        private readonly ICommand _recoverCommand;
        private readonly ICommand _loginCommand;
        private readonly INavigationService _navigationService;

        public RecoverUserViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");

            _recoverCommand = new DelegateCommand(parameter => OnClickRecover());
            _loginCommand = new DelegateCommand(parameter => OnClickLogin());
            _getOTPCommand = new DelegateCommand(parameter => OnClickGetOTP());
        }

        private void OnClickGetOTP()
        {
            if (ValidateProperties())
            {
                _isRecoverEnable = true;
                _generatedOTP = Utilities.GenerateOTP();

                ContactEmail contactEmail = new ContactEmail
                {
                    Address = _emailId,
                    Description = _generatedOTP,
                    Kind = ContactEmailKind.Work
                };

                Contact contact = new Contact();
                contact.Emails.Add(contactEmail);

                ContactPhone contactPhone = new ContactPhone
                {
                    Number = _mobileNo
                };

                contact.Phones.Add(contactPhone);

                ComposeSms(contact);

                OnPropertyChanged(() => IsRecoverEnable);
            }
        }

        private async void ComposeSms(Contact recipient)
        {
            var chatMessage = new Windows.ApplicationModel.Chat.ChatMessage();
            chatMessage.Body = "this is dv maurya";

            var phone = recipient.Phones.FirstOrDefault();
            if (phone != null)
            {
                chatMessage.Recipients.Add(phone.Number);
            }
            await Windows.ApplicationModel.Chat.ChatMessageManager.ShowComposeSmsMessageAsync(chatMessage);
        }

        private void OnClickLogin()
        {
            _navigationService.Navigate(typeof(MainPage));
        }

        private void OnClickRecover()
        {
            if (ValidateProperties())
            {

            }
        }

        [EmailValidationAttribute(ErrorMessage = ApplicationConstants.InvalidEmail)]
        public string EmailId
        {
            get => _emailId;
            set => _emailId = value;
        }

        [MobileNoValidationAttribute(ErrorMessage = ApplicationConstants.InvalidMobileNo)]
        public string MobileNo
        {
            get => _mobileNo;
            set => _mobileNo = value;
        }

        [OTPValidationAttribute(ErrorMessage = ApplicationConstants.InvalidOTP)]
        public string OTP
        {
            get => _oTP;
            set => _oTP = value;
        }

        public bool IsRecoverEnable
        {
            get => _isRecoverEnable;
            set => _isRecoverEnable = value;
        }

        public string GeneratedOTP
        {
            get => _generatedOTP;
            set => _generatedOTP = value;
        }

        public ICommand RecoverCommand => _recoverCommand;
        public ICommand LoginCommand => _loginCommand;
        public ICommand GetOTPCommand => _getOTPCommand;
    }
}
