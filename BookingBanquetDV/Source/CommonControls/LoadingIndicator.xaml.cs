using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace BookingBanquetDV.Source.CommonControls
{
    public sealed partial class LoadingIndicator : UserControl
    {
        public LoadingIndicator()
        {
            this.InitializeComponent();
        }

        public void ShowLoadingIndicator(string contentText)
        {
            this.Visibility = Visibility.Visible;
            this.loadingText.Text = contentText;
        }

        public void ChangeLoadingIndicatorText(string contextText)
        {
            this.loadingText.Text = contextText;
        }

        public void HideLoadingIndicator()
        {
            this.Visibility = Visibility.Collapsed;
        }
    }
}
