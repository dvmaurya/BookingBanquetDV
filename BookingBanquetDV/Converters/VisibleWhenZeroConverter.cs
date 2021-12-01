using CommonServices.Extensions;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace BookingBanquetDV.Converters
{
    public class VisibleWhenZeroConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return Equals(0, value.ToInt32()) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
