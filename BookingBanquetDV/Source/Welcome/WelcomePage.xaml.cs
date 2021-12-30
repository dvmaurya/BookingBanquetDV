using CommonServices.Enums;
using CommonServices.Extensions;
using CommonServices.Utilities;
using ViewModelsAndValidation;
using ViewModelsAndValidation.Views;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace BookingBanquetDV.Source.Welcome
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WelcomePage : Page, IView
    {
        public WelcomePage()
        {
            this.InitializeComponent();
            this.DataContext = ViewModelLocator.WelcomeViewModel;
        }

        public ViewModelBase ViewModel => this.DataContext as WelcomeViewModel;

        private void AppBarButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if(sender != null)
            {
                if (sender is AppBarButton appBar)
                {
                    int id = EnumUtility.GetValueFromDescription<BookingActionEnum>(appBar.Label).ToInt32();
                    CursorUtility.CustomCursor(id);
                }
                else if(sender is Button button || sender is CommandBar commandBar)
                {
                    CursorUtility.HandCursor();
                }
            }
        }

        private void AppBarButton_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            CursorUtility.ArrowCursor();
        }
    }
}
