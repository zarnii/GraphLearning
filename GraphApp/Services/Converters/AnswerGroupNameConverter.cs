using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace GraphApp.Services.Converters
{
    public class AnswerGroupNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var source = (ItemsControl)value;

            return source.DataContext.GetHashCode();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
