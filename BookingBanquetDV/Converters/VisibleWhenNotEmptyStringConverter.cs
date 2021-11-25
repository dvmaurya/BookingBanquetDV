using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
namespace BookingBanquetDV.Converters
{
    public class VisibleWhenNotEmptyStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return Visibility.Collapsed;
            else
                value = value.ToString().Trim();

            return string.IsNullOrEmpty(value.ToString()) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
