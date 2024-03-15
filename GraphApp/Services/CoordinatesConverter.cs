using System;
using System.Globalization;
using System.Windows.Data;

namespace GraphApp.Services
{
    public class CoordinatesConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var coordinate = (double)values[0];
            var diameter = (int)values[1] * 2;

            return coordinate + diameter;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
