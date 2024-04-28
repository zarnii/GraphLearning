using System;
using System.Globalization;
using System.Windows.Data;

namespace GraphApp.Services.Converters
{
    /// <summary>
    /// Конвертор кординат названия вершины.
    /// </summary>
    public class VertexNameCoordinatesConverter : IValueConverter
    {
        /// <summary>
        /// Отклонение по умолчанию.
        /// </summary>
        private const int DefaultRejection = 20;

        /// <summary>
        /// Конвертирование координаты вершины в координаты названия вершины.
        /// </summary>
        /// <param name="value">Координата.</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var coordinate = (double)value;

            return coordinate - DefaultRejection;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
