using CommonServices.Extensions;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace BookingBanquetDV.Converters
{
    public class NotBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
                return !value.ConvertToString().ConvertToBoolean() ? Visibility.Visible : Visibility.Collapsed;

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
