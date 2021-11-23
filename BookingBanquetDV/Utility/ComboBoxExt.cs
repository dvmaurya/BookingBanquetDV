using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BookingBanquetDV.Utility
{
    public class ComboBoxExt : ComboBox
    {
        private ScrollViewer _scrollViewer;

        public ComboBoxExt()
        {
            DefaultStyleKey = typeof(ComboBox);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _scrollViewer = GetTemplateChild("ScrollViewer") as ScrollViewer;
            if (_scrollViewer != null)
            {
                _scrollViewer.Loaded += OnScrollViewerLoaded;
                this.DropDownOpened += ComboBoxExt_DropDownOpened;
            }
        }

        private void ComboBoxExt_DropDownOpened(object sender, object e)
        {
            if (_scrollViewer != null)
            {
                if (this.SelectedItem == null)
                    _scrollViewer.ChangeView(null, 0, null);
            }

        }

        private void OnScrollViewerLoaded(object sender, RoutedEventArgs e)
        {
            if (_scrollViewer != null)
            {
                _scrollViewer.Loaded -= OnScrollViewerLoaded;

                if (this.SelectedItem == null)
                    _scrollViewer.ChangeView(null, 0, null);
            }
        }
    }
}
