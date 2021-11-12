using System;
using CommonServices.Extensions;
using Windows.UI.Xaml.Data;

namespace BookingBanquetDV.Converters
{
    public class InverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return !value.ConvertToBoolean();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
