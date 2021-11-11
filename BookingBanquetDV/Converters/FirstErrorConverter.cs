using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Data;

namespace BookingBanquetDV.Converters
{
    public class FirstErrorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return
                value is ICollection<string> errors && errors.Count > 0
                ?
                errors.ElementAt(0)
                :
                null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
