using System;
using System.Globalization;

namespace CommonServices.Extensions
{
    public static class CultureCovertion
    {
        private static readonly CultureInfo cultureInfo = CultureInfo.InvariantCulture;
        private const StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase;

        public static int ToInt16(this object value)
        {
            return Convert.ToInt16(value, cultureInfo);
        }

        public static int ToInt32(this object value)
        {
            return Convert.ToInt32(value, cultureInfo);
        }

        public static long ToInt64(this object value)
        {
            return Convert.ToInt64(value, cultureInfo);
        }

        public static ulong UlongParse(this object value)
        {
            return ulong.Parse(ConvertToString(value), cultureInfo);
        }

        public static long TolongParse(this object value)
        {
            return long.Parse(ConvertToString(value), cultureInfo);
        }

        public static double ConvertToDouble(this object value)
        {
            return Convert.ToDouble(value, cultureInfo);
        }

        public static string ConvertToString(this object value)
        {
            return Convert.ToString(value, cultureInfo);
        }

        public static string FormatString(this string value, params object[] formats)
        {
            return string.Format(cultureInfo, value, formats);
        }

        public static string ConcatString(this string value, params object[] formats)
        {
            return string.Concat(cultureInfo, value, JoinString(formats));
        }

        private static string JoinString(params object[] formats)
        {
            return string.Join("", formats);
        }

        public static bool ConvertToBoolean(this object value)
        {
            return Convert.ToBoolean(value, cultureInfo);
        }

        public static string ConvertToString(this DateTime dateTime)
        {
            return dateTime.ToString(cultureInfo);
        }

        public static decimal? Normalize(this decimal? value)
        {
            if (value.HasValue)
            {
                return value.Value / 1.000000000000000000000000000000000m;
            }
            return value;
        }

        public static string ConvertToString(this DateTime dateTime, string format)
        {
            return dateTime.ToString(format, cultureInfo);
        }

        public static string ConvertToString(this DateTimeOffset dateTime, string format)
        {
            return dateTime.ToString(format, cultureInfo);
        }

        public static DateTimeOffset ConvertToDate(this string value)
        {
            return DateTimeOffset.Parse(value, cultureInfo);
        }

        public static DateTime ConvertToDateTime(this string value, string format)
        {
            return DateTime.ParseExact(value, format, cultureInfo);
        }

        public static DateTime ConvertToDateTime(this string value)
        {
            return Convert.ToDateTime(value, cultureInfo);
        }

        public static TimeSpan ConvertToTimeSpan(this string value)
        {
            return TimeSpan.Parse(value, cultureInfo);
        }

        public static DateTime ConvertToDateTime(this string value, string[] format)
        {
            return DateTime.ParseExact(value, format, cultureInfo, DateTimeStyles.None);
        }

        public static bool FormatStringEquals(this string value, string format)
        {
            return string.Equals(value, format, stringComparison);
        }

        public static bool CheckedStartsWith(this string value, string format)
        {
            return value.StartsWith(format, stringComparison);
        }

        public static bool CheckedEndsWith(this string value, string format)
        {
            return value.EndsWith(format, stringComparison);
        }

        public static int GetIndexOf(this string value, string format)
        {
            return value.IndexOf(format, stringComparison);
        }

        public static DateTime ConvertToDateTimeTryParse(this string dateTime)
        {
            if (DateTime.TryParse(dateTime, out DateTime result))
                return result;

            return new DateTime();
        }

        public static decimal ConvertToDecimal(this object value)
        {
            return Convert.ToDecimal(value, cultureInfo);
        }

        public static string ConvertToString(this decimal? value, string format)
        {
            if (value.HasValue)
            {
                return value.Value.ToString(format, cultureInfo);
            }

            return null;
        }
    }
}
