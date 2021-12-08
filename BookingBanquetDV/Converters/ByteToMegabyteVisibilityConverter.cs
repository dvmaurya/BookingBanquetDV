using BookingBanquetDV.Utility;
using CommonServices.Extensions;
using System;
using Windows.UI.Xaml.Data;

namespace BookingBanquetDV.Converters
{
    public class ByteToMegabyteVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string size = "0";

            if (value != null)
                size = ByteConverter.DocumentSize(value.ToInt32());

            return size;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}