using Windows.UI.Xaml.Controls;

namespace CommonServices.Printout
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
