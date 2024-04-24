using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace GraphApp.Services.Converters
{
    /// <summary>
    /// Конвертор цвета.
    /// </summary>
    public class ColorConverter : IValueConverter
    {
        /// <summary>
        /// Конвертирование из SolidColorBrush в Color.
        /// </summary>
        /// <param name="value">Цвет.</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var solidColorBrush = (SolidColorBrush)value;

            return solidColorBrush.Color;
        }

        /// <summary>
        /// Конвертирование из Color в SolidColorBrush.
        /// </summary>
        /// <param name="value">Цвет.</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = (Color)value;

            return new SolidColorBrush(color);
        }
    }
}
