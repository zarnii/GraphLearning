﻿using GraphApp.Model;
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
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {   
            var coord1 = (double)values[0];
            var coord2 = (double)values[1];
            var connection = (VisualConnection)values[2];

            if (connection.FirstConnectedVertex == connection.SecondConnectedVertex)
            {
                return (coord1 + connection.FirstConnectedVertex.Radius + coord2 + connection.FirstConnectedVertex.Radius) / 2;
            }

            return (coord1 + coord2) / 2;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
