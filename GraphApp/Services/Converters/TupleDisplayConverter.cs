using System;
using System.Globalization;
using System.Windows.Data;

namespace GraphApp.Services.Converters
{
    public class TupleDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var tuple = ((string, int))value;
            
            return (string)parameter == "Item1"
                ? tuple.Item1
                : tuple.Item2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
