using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;
using GraphApp.Model;
using Petzold.Media2D;

namespace GraphApp.Services.Converters
{
    public class ConnectionTypeVisualisator : IValueConverter
    {
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
