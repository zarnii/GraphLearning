using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GraphApp.Services.Converters
{
    public class ConnectionCoordinatesConverter : IMultiValueConverter
    {
        /// <summary>
        /// Конвертор координат связей.
        /// </summary>
        /// <param name="values">Coord1, Coord2, Coord3, Coord4, Radius</param>
        /// <param name="targetType"></param>
        /// <param name="parameter">Конвертируемая координата (X или Y) в виде строки.</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var x1 = (double)values[0];
            var y1 = (double)values[1];
            var x2 = (double)values[2];
            var y2 = (double)values[3];
            var radius = (int)values[4];

            var deltaX = x2 - x1;
            var deltaY = y2 - y1;

            double angleInRadians = Math.Atan2(deltaY, deltaX);
            
            if ((String)parameter == "X")
            {
                return x2 - radius * Math.Cos(angleInRadians);
            }

            if ((String)parameter == "Y")
            {
                return y2 - radius * Math.Sin(angleInRadians);
            }

            return null;
            
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
