﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphApp.Model
{
    public class IncidenceMatrix: BaseMatrix
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

            Matrix = new int[rows.Length][];

            for (var i = 0; i < Matrix.Length; i++)
            {
                Matrix[i] = new int[colums.Length];
            }

            foreach (var row in rows)
            {
                var firstIndex = row.ConnectedVertices.Item1.Number - 1;
                var secondIndex = row.ConnectedVertices.Item2.Number - 1;

                if (firstIndex == secondIndex)
                {
                    Matrix[firstIndex][secondIndex] = 2;
                }

                Matrix[row.Number - 1][firstIndex] = -1;
                Matrix[row.Number - 1][secondIndex] = 1;
            }

            ColumnsDescription = colums.Select(v => v.Name).ToArray();
            RowsDescription = rows.Select(c => c.Number.ToString()).ToArray();
        }
    }
}
