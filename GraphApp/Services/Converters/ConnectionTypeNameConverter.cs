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
    class ConnectionTypeNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is String)
            {
                return null;
            }

            var connectionType = (ConnectionType)value;

            if (connectionType == null)
            {
                return null;
            }

            if (connectionType == ConnectionType.NonDirectional)
            {
                return "Ненаправленный";
            }
            else if (connectionType == ConnectionType.Unidirectional)
            {
                return "Однонаправленный";
            }
            else if (connectionType == ConnectionType.Bidirectional)
            {
                return "Двунаправленный";
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
