using GraphApp.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GraphApp.Services.Converters
{
    /// <summary>
    /// Конвертор координат веса связи.
    /// </summary>
    public class ConnectionWeightCoordinatesConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {   
            var x = (double)values[0];
            var y = (double)values[1];
            var connection = (VisualConnection)values[2];

            if (connection.FirstConnectedVertex == connection.SecondConnectedVertex)
            {
                return (x + connection.FirstConnectedVertex.Radius + y + connection.FirstConnectedVertex.Radius) / 2;
            }

            return (x + y) / 2;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
