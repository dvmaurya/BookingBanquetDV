using Windows.UI.Xaml.Controls;

namespace BookingBanquetDV.Source.Welcome.PrintOut
{
    public sealed partial class ContinuationPage : Page
    {
        public ContinuationPage(RichTextBlockOverflow textLinkContainer)
        {
            InitializeComponent();
            textLinkContainer.OverflowContentTarget = ContinuationPageLinkedContainer;
        }
    }
}
