using CommonServices.Extensions;
using System;

namespace BookingBanquetDV.Utility
{
    public static class ByteConverter
    {
        public static string DocumentSize(int value)
        {
            string[] units = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            double size = value.ConvertToDouble();

            while (size >= 1024 && order < units.Length - 1)
            {
                order++;
                size = size / 1024;
            }

            size = Math.Round(size);

            string result = $"{size:0.##} {units[order]}";
            return result;
        }
    }
}
