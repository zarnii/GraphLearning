﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphApp.Model
{
    /// <summary>
    /// Матрица смежности.
    /// </summary>
    public class AdjacencyMatrix: BaseMatrix
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="vertexCollection">Коллекция вершин.</param>
        /// <param name="connectionCollection">Коллекция связей.</param>
        public AdjacencyMatrix(ICollection<Vertex> vertexCollection, ICollection<Connection> connectionCollection)
        {
            if (vertexCollection == null)
            {
                throw new ArgumentNullException(nameof(vertexCollection), "Пустая коллекция вершин.");
            }

            if (connectionCollection == null)
            {
                throw new ArgumentNullException(nameof(connectionCollection), "Пустая коллекция связей.");
            }

            var colums = vertexCollection.OrderBy(v => v.Number).ToArray();

            var rows = new Vertex[vertexCollection.Count];
            Array.Copy(colums, rows, colums.Length);

            Matrix = new int[colums.Length, rows.Length];

            foreach (var connection in connectionCollection)
            {
                var firstIndex = connection.ConnectedVertices.Item1.Number - 1;
                var secondIndex = connection.ConnectedVertices.Item2.Number - 1;

                var weight = connection.Weight == 0
                    ? 1 
                    : connection.Weight;

                Matrix[firstIndex, secondIndex] = (int)weight;
                Matrix[secondIndex, firstIndex] = (int)weight;
            }

            ColumnsDescription = colums.Select(v => v.Name).ToArray();
            RowsDescription = new string[rows.Length];
            Array.Copy(ColumnsDescription, RowsDescription, ColumnsDescription.Length);
        }
    }
}
