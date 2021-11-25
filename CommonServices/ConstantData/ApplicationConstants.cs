namespace CommonServices.ConstantData
{
    public static class ApplicationConstants
    {
        public const string LogRetainDays = "30";
        public const string AppVersion = "App Version";
        public const string ToastNotification = "ToastNotification";
        public const string UserNameRequired = "Enter user name";
        public const string NameRequired = "Enter full name";
        public const string EmailRequired = "Enter email id";
        public const string EmailIdExist = "This email id is already exist. Please use different email id.";
        public const string EmailRegex = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
        public const string InvalidEmail = "Enter valid email id";
        public const string MobileExist = "This mobile number is already exist. Please use different mobile number.";
        public const string MobileRequired = "Enter mobile number";
        public const string MobileRegex = "^[6-9]\\d{9}$";
        public const string InvalidMobileNo = "Enter valid mobile number";
        public const string OTPRequired = "Enter one time password";
        public const string InvalidOTP = "Enter valid one time password";
        public const string PasswordRequired = "Enter password";
        public const string ConfirmPassword = "Enter confirm password";
        public const string PasswordMatch = "Both passwords should be same";
        public const string PasswordLength = "Password length should be minimum 3";
        public const string DatabaseName = "Banquet.sqlite";
        public const string DatabaseDateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        public const string DatabaseDateFormat = "yyyy-MM-dd";
        public const int DefaultCommandIndex = 0;
        public const int CancelCommandIndex = 1;
        public const string Success = "Success";
        public const string Warning = "Warning";
        public const string Error = "Error";
        public const string CredentialWrong = "Either username or password is incorrect";
        public const string AccountNotActivated = "This account has not been activated. Please contact with your administrator for activating.";
        public const string SignupSuccess = "Successfully created your account. Please go to login page.";
        public const string Users = "Users";
        public const string LoginDetail = "LoginDetail";
        public const string BookingDetails = "BookingDetails";
        public const string LowInternet = "You are in low internet area. Please connect with internet.";
        public const string PasswordUpdated = "Password updated successfully.";
        public const string Close = "Close";
        public const string DateRequired = "Select booked date";
        public const string UnableToOpenCamera = "Unable to open camera";
        public const string FileFormat = ".jpeg";
		public const string UnableToSavePhoto = "Unable to save the photo.";
        public const string Document = "Document";
        public const string TotalAmountError = "Enter total amount";
        public const string AdvanceAmountError = "Enter advance amount";
        public const string DateBooked = "This date has already booked. Please choose another date.";
    }
}
