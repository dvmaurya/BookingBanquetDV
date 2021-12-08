using CommonServices.Model.City;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace BookingBanquetDV.Source.CommonControls
{
    public sealed partial class SingleSelectorDialog : ContentDialog
    {
        private SingleSelection _selectedCode;

        private readonly IReadOnlyCollection<SingleSelection> _singleSelection;

        public SingleSelectorDialog(IReadOnlyCollection<SingleSelection> codes, string header)
        {
            this.InitializeComponent();

            SingleSelectionTitle.Title = "Select " + header;

            _singleSelection = codes ?? throw new ArgumentNullException("codes");
            this.lstCodes.ItemsSource = _singleSelection;

            if (codes.Count != 0)
            {
                NoVisibility.Visibility = Visibility.Collapsed;
                AllVisibility.Visibility = Visibility.Visible;
            }
            else
            {
                NoVisibility.Visibility = Visibility.Visible;
                AllVisibility.Visibility = Visibility.Collapsed;
            }
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (sender != null && args != null)
            {
                _selectedCode = null;
                this.Hide();
            }
        }

        private void LstCodes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView listview && e != null)
            {
                _selectedCode = listview.SelectedItem as SingleSelection;
            }

            this.Hide();
        }

        private void SearchTemplate_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (_singleSelection != null && args != null)
            {
                lstCodes.ItemsSource =
                    _singleSelection
                    .Where(
                        code =>
                            code.Name.ToLower().Contains(sender.Text.ToLower()))
                    .ToList();
            }
        }

        public SingleSelection SelectedCode => _selectedCode;
    }
}
