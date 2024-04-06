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
        /// <param name="values">Конвертируемые значения.</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>Координаты.</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var coordinate = (double)values[0];
            var diameter = (int)values[1] * 2;

            if ((string)parameter == "X")
            {
                return coordinate + diameter;
            }

            return coordinate - diameter;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
