using CommonServices.Model;
using CommonServices.Utilities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ViewModelsAndValidation.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace BookingBanquetDV.Source.Welcome.Controls
{
    public sealed partial class AllProfilePhoto : ContentDialog
    {
        private IList<PhotoDetails> _photoDetails;
        private PhotoDetails _selectedPhotoDetail;

        public AllProfilePhoto(IList<PhotoDetails> photoDetails)
        {
            this.InitializeComponent();
            _photoDetails = photoDetails;

            this.DataContext = ViewModelLocator.AllProfilePhotoViewModel;
        }

        private void DisposeContentDialogs(object sender, DisposeDialogEventArgs e)
        {
            this.Hide();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (sender != null &&
                args != null &&
                this.DataContext is IAllProfilePhotoViewModel viewModel)
                _selectedPhotoDetail = viewModel.SelectedPhotoDetail;

            this.Hide();
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            this.Hide();
        }

        public PhotoDetails SelectedPhotoDetail => _selectedPhotoDetail;

        private void ContentDialog_Loading(FrameworkElement sender, object args)
        {
            if (this.DataContext is IAllProfilePhotoViewModel viewModel)
            {
                viewModel.PhotoDetailList = new ReadOnlyCollection<PhotoDetails>(_photoDetails);
                viewModel.OnDialogLoadingCommand.Execute(null);
            }
        }
    }
}
