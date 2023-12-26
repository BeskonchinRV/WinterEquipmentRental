using System;
using System.Globalization;
using System.Windows.Data;

namespace WinterEquipmentRental
{
    public class RemainingTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime endTime)
            {
                TimeSpan remainingTime = endTime - DateTime.Now;
                return $"{remainingTime.Hours:D2}:{remainingTime.Minutes:D2}:{remainingTime.Seconds:D2}";
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
