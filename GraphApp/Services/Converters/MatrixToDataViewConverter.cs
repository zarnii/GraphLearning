using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using GraphApp.Model;

namespace GraphApp.Services.Converters
{
    public class MatrixToDataViewConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            var dataTable = new DataTable();
            var matrix = (BaseMatrix)value;

            dataTable.Columns.Add("----");

            foreach (var row in matrix.ColumnsDescription)
            {
                dataTable.Columns.Add(row);
            }

            var index = 0;

            foreach (var row in matrix.Matrix)
            {
                var stringRow = new List<string>(1 + row.Length);
                stringRow.Add(matrix.RowsDescription[index]);

                foreach (var number in row)
                {
                    stringRow.Add(number.ToString());
                }

                index++;
                dataTable.Rows.Add(stringRow.ToArray());
            }

            return dataTable.DefaultView;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
