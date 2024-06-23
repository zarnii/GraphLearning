using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphApp.Model
{
    /// <summary>
    /// Матрица инцидентности.
    /// </summary>
    public class IncidenceMatrix : BaseMatrix
    {
        public IncidenceMatrix(ICollection<Vertex> vertexCollection, ICollection<Connection> connectionCollection)
        {
            if (vertexCollection == null)
            {
                throw new ArgumentNullException(nameof(vertexCollection));
            }

            if (connectionCollection == null)
            {
                throw new ArgumentNullException(nameof(connectionCollection));
            }

            var colums = vertexCollection.OrderBy(v => v.Number).ToArray();
            var rows = connectionCollection.OrderBy(c => c.Number).ToArray();

            Matrix = new int[rows.Length, colums.Length];

            foreach (var row in rows)
            {
                var rowIndex = row.Number - 1;
                var firstColumnIndex = row.ConnectedVertices.Item1.Number - 1;
                var secondColumnIndex = row.ConnectedVertices.Item2.Number - 1;

                var firstValue = row.ConnectionType == ConnectionType.Unidirectional
                    ? -1
                    : 1;
                var secondValue = 1;


                if (firstColumnIndex == secondColumnIndex)
                {
                    Matrix[rowIndex, secondColumnIndex] = 2;
                }
                else
                {
                    Matrix[rowIndex, firstColumnIndex] = firstValue;
                    Matrix[rowIndex, secondColumnIndex] = secondValue;
                }
            }

            ColumnsDescription = colums.Select(v => v.Name).ToArray();
            RowsDescription = rows.Select(c => c.Number.ToString()).ToArray();
        }
    }
}
