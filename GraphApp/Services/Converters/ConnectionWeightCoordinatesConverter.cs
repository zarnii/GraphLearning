using GraphApp.Model;
using System;
using System.Globalization;
using System.Windows.Data;

namespace GraphApp.Services.Converters
{
    /// <summary>
    /// Конвертор координат веса связи.
    /// </summary>
    public class ConnectionWeightCoordinatesConverter : IMultiValueConverter
    {
        /// <summary>
        /// Отклонение по умолчанию.
        /// </summary>
        private const int DefaultRejection = 10;

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {   
            var coord1 = (double)values[0];
            var coord2 = (double)values[1];
            var firstConnectedVertex = (VisualVertex)values[2];
            var secondConnectedVertex = (VisualVertex)values[3];

            if (firstConnectedVertex == secondConnectedVertex)
            {
                return ((coord1 + coord2 + firstConnectedVertex.Radius * 2) / 2) + DefaultRejection;
            }

            return (coord1 + coord2) / 2;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
