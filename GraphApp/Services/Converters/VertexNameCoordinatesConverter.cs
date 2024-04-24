using System;
using System.Globalization;
using System.Windows.Data;

namespace GraphApp.Services.Converters
{
    /// <summary>
    /// Конвертор кординат названия вершины.
    /// </summary>
    public class VertexNameCoordinatesConverter : IMultiValueConverter
    {
        /// <summary>
        /// Конвертирование.
        /// </summary>
        /// <param name="values">Координата (X или Y) и радиус.</param>
        /// <param name="targetType"></param>
        /// <param name="parameter">Строка, показывающая какая коордианта была передана (X или Y).</param>
        /// <param name="culture"></param>
        /// <returns>Координаты.</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var coordinate = (double)values[0];
            var diameter = (int)values[1];


            return coordinate - 20;
 
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
