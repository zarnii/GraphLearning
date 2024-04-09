using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphApp.Model;

namespace GraphApp.Model
{
    /// <summary>
    /// Матрица смежности.
    /// </summary>
    public class AdjacencyMatrix
    {
        /// <summary>
        /// Описание вершин в верхней части матрицы.
        /// </summary>
        public string[] TopSideDescription { get; private set; }

        /// <summary>
        /// Описание вершмн в нижней части матрицы.
        /// </summary>
        public string[] LeftSideDescription { get; private set; }

        /// <summary>
        /// Матрица смежности.
        /// </summary>
        public byte[,] Matrix { get; private set; }

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

            var topSide = new Vertex[vertexCollection.Count];
            topSide = vertexCollection.OrderBy(v => v.Number).ToArray();

            var leftSide = new Vertex[vertexCollection.Count];
            Array.Copy(topSide, leftSide, topSide.Length);

            Matrix = new byte[topSide.Length, leftSide.Length];

            foreach (var connection in connectionCollection)
            {
                var firstIndex = connection.ConnectedVertices.Item1.Number - 1;
                var secondIndex = connection.ConnectedVertices.Item2.Number - 1;
                Matrix[firstIndex, secondIndex] = 1;
                Matrix[secondIndex, firstIndex] = 1;
            }

            TopSideDescription = topSide.Select(v => v.Name).ToArray();
            LeftSideDescription = new string[leftSide.Length];
            Array.Copy(TopSideDescription, LeftSideDescription, TopSideDescription.Length);
        }
    }
}
