using CommonServices.Extensions;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace BookingBanquetDV.Source.Register
{
    public sealed partial class NotesControl : UserControl
    {
        private const string ExceedLimitError = "Exceeds character limit ( -{0} )";

        public NotesControl()
        {
            this.InitializeComponent();
            txtNotesLength.Visibility = Visibility.Collapsed;
        }

        private void ShowNotesLength(object sender, object e)
        {
            txtNotesLength.Visibility = Visibility.Visible;

            if(sender != null && e != null) { }
        }

        private void AddNotes_LostFocus(object sender, RoutedEventArgs e)
        {
            txtNotesLength.Visibility = Visibility.Collapsed;

            if (sender != null && e != null) { }
        }

        public string NotesText
        {
            get { return (string)GetValue(ControlRenderModeProperty); }
            set { SetValue(ControlRenderModeProperty, value); }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty ControlRenderModeProperty =
            DependencyProperty.Register("NotesText", typeof(string), typeof(NotesControl), new PropertyMetadata(null, OnPropertyChanged));

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                NotesControl notesControl = d as NotesControl;
                long notesLength = e.NewValue.ToString().Length;
                if (notesControl.DataContext is INotesValidator notesValidator)
                {
                    notesControl.MaxLength = notesValidator.MaxLength.ToInt32();
                }

                CheckExceedLimit(notesControl, notesLength, notesControl.MaxLength);
            }
        }

        private static void CheckExceedLimit(NotesControl notesControl, long notesLength, long maxLength)
        {
            if (notesLength > maxLength)
            {
                string count = (notesLength - maxLength).ConvertToString();
                notesControl.ExceedBlock.Text = ExceedLimitError.FormatString(count);
                notesControl.ExceedBlock.Visibility = Visibility.Visible;
                notesControl.addNotes.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                notesControl.ExceedBlock.Visibility = Visibility.Collapsed;
                notesControl.addNotes.BorderBrush = (SolidColorBrush)(App.Current.Resources["NotesControlBorderColor"]);
            }
        }

        public string PlaceholderText
        {
            get { return (string)GetValue(PlaceholderTextRenderModeProperty); }
            set { SetValue(PlaceholderTextRenderModeProperty, value); }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty PlaceholderTextRenderModeProperty =
            DependencyProperty.Register("PlaceHolderText", typeof(string), typeof(NotesControl), new PropertyMetadata(null, OnPlaceHolderTextPropertyChanged));

        private static void OnPlaceHolderTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {

            }
        }

        public int MaxLength
        {
            get { return (int)GetValue(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty MaxLengthProperty =
            DependencyProperty.Register("MaxLength", typeof(int), typeof(NotesControl), new PropertyMetadata(0));

        public double NotesControlWidth
        {
            get { return (double)GetValue(NotesControlWidthRenderModeProperty); }
            set { SetValue(NotesControlWidthRenderModeProperty, value); }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty NotesControlWidthRenderModeProperty =
            DependencyProperty.Register("NotesControlWidth", typeof(double), typeof(NotesControl), new PropertyMetadata(632, OnNotesControlWidthPropertyChanged));

        private static void OnNotesControlWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {

            }
        }
    }
}
