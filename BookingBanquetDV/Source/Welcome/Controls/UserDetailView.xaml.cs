using CommonServices.Model;
using ViewModelsAndValidation.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace BookingBanquetDV.Source.Welcome.Controls
{
    public sealed partial class UserDetailView : ContentDialog
    {
        private Users _users;

        public UserDetailView(Users userDetail)
        {
            this.InitializeComponent();
            this.DataContext = ViewModelLocator.userDetailViewModel;

            _users = userDetail;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            this.Hide();
        }

        private void ContentDialog_Loading(FrameworkElement sender, object args)
        {
            if (sender != null &&
                this.DataContext is IUserDetailViewModel viewModel)
            {
                viewModel.Users = _users;
                viewModel.OnDialogLoadCommand.Execute(null);
            }

            if (args != null) { }
        }
    }
}
