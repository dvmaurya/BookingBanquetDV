using Windows.Foundation;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace BookingBanquetDV.Source.Camera
{
    public sealed partial class PreviewPhoto : ContentDialog
    {
        public PreviewPhoto(CommonServices.Model.PhotoDetails photoDetails)
        {
            this.InitializeComponent();
            previewImage.Source = photoDetails.ImageSource;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            this.Hide();
        }

        private void ScrollViewer_DoubleTapped(object sender, Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            ScrollViewer scrollViewer = sender as ScrollViewer;
            Point doubleTapPoint = e.GetPosition(scrollViewer);

            if (scrollViewer.ZoomFactor != 1)
                scrollViewer.ChangeView(doubleTapPoint.X, doubleTapPoint.Y, 1);
            else if (scrollViewer.ZoomFactor == 1)
                scrollViewer.ChangeView(doubleTapPoint.X, doubleTapPoint.Y, 2);
        }
    }
}
