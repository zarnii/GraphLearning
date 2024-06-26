﻿using GraphApp.Model;
using System;
using System.Globalization;
using System.Windows.Data;

namespace GraphApp.Services.Converters
{
    public class ConnectionNumberCoordinatesConverter : IMultiValueConverter
    {
        private int _defaultDeviationY = 20;

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var coord1 = (double)values[0];
            var coord2 = (double)values[1];
            var firstConnectedVertex = (VisualVertex)values[2];
            var secondConnectedVertex = (VisualVertex)values[3];

            if (firstConnectedVertex == secondConnectedVertex)
            {
                return (coord1 + firstConnectedVertex.Radius + coord2 + firstConnectedVertex.Radius) / 2;
            }

            if ((string)parameter == "Y")
            {
                return ((coord1 + coord2) / 2) - _defaultDeviationY;
            }

            return ((coord1 + coord2) / 2);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
