using Huyn.MultiBinding;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookingBanquetDV.Converters
{
    public class NumberingConvertor : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, string language)
        {
            if (values != null && values.Any() && values[0] != null && values[1] != null)
            {
                //return (char)(((List<object>)values[1]).IndexOf(values[0]) + 97);
                return ((List<object>)values[1]).IndexOf(values[0]) + 1;
            }
            return "0";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, string language)
        {
            return null;
        }
    }
}
