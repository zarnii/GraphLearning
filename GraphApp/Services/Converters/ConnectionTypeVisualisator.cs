using GraphApp.Model;
using Petzold.Media2D;
using System;
using System.Globalization;
using System.Windows.Data;

namespace GraphApp.Services.Converters
{
    public class ConnectionTypeVisualisator : IValueConverter
    {
        /// <summary>
        /// Преобразование ConnectionType в окончание связи.
        /// </summary>
        /// <param name="value">Тип связи.</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>ArrowEnds.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var connectionType = (ConnectionType)value;

            if (connectionType == ConnectionType.NonDirectional)
            {
                return ArrowEnds.None;
            }

            if (connectionType == ConnectionType.Unidirectional)
            {
                return ArrowEnds.End;
            }

            return ArrowEnds.Both;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
