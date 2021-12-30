using CommonServices.Model;
using ViewModelsAndValidation.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace BookingBanquetDV.Source.Welcome.Controls
{
    public sealed partial class BanquetConfigDialog : ContentDialog
    {
        private LoginDetail _loginDetail;
        private BanquetDetail _banquetDetail;

        public BanquetConfigDialog(LoginDetail loginDetail, BanquetDetail banquetDetail)
        {
            this.InitializeComponent();
            _loginDetail = loginDetail;
            _banquetDetail = banquetDetail;

            this.DataContext = ViewModelLocator.BanquetConfigViewModel;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (this.DataContext is IBanquetConfigViewModel viewModel)
            {
                viewModel.SaveDetailCommand.Execute(null);
            }

            this.Hide();
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            this.Hide();
        }

        private void ContentDialog_Loading(FrameworkElement sender, object args)
        {
            if(this.DataContext is IBanquetConfigViewModel viewModel)
            {
                viewModel.LoginDetail = _loginDetail;
                viewModel.BanquetDetail = _banquetDetail;
                viewModel.DialogLoadingCommand.Execute(null);
            }
        }
    }
}
