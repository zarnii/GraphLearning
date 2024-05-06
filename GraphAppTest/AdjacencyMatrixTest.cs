using GraphApp.Model;
using System.Diagnostics;

namespace GraphAppTest
{
    [TestClass]
    public class AdjacencyMatrixTest
    {

        [TestMethod]
        [DynamicData(nameof(GenerateDate), DynamicDataSourceType.Method)]
        public void AdjacencyMatrix_Create_Success(
            ICollection<Vertex> vertices, 
            ICollection<Connection> connections,
            int[,] expectedMatrix)
        {
            
            var matrix = new AdjacencyMatrix(vertices, connections);
            var actualMatrix = matrix.Matrix;

            var expectedRows = expectedMatrix.GetUpperBound(0) + 1;
            var expectedColumns = expectedMatrix.Length / expectedRows;
            
            var actualRows = actualMatrix.GetUpperBound(0) + 1;
            var actualColumns = actualMatrix.Length / actualRows;
            
            Assert.AreEqual(expectedColumns, actualColumns);

            CollectionAssert.AreEqual(expectedMatrix, actualMatrix);
        }

        private static IEnumerable<object?[]> GenerateDate()
        {
            var _1 = new Vertex(0, 0, 1, "1");
            var _2 = new Vertex(0, 0, 2, "2");
            var _3 = new Vertex(0, 0, 3, "3");
            var _4 = new Vertex(0, 0, 4, "4");
            var _5 = new Vertex(0, 0, 5, "5");
            var _6 = new Vertex(0, 0, 6, "6");
            var _7 = new Vertex(0, 0, 7, "7");

            yield return new object[3]
            {
                new List<Vertex>(3)
                { 
                    _1,
                    _2,
                    _3
                },
                new List<Connection>(3)
                {
                    new Connection((_1, _2), 1),
                    new Connection((_2, _3), 2),
                    new Connection((_3, _1), 3)
                },
                new int[,]
                {
                    { 0, 1, 1 },
                    { 1, 0, 1 },
                    { 1, 1, 0 }
                }
            };
            yield return new object[3]
            {
                new List<Vertex>(5)
                {
                    _1,
                    _2,
                    _3,
                    _4,
                    _5
                },
                new List<Connection>(5)
                { 
                    new Connection((_1, _5), 1),
                    new Connection((_5, _3), 2),
                    new Connection((_3, _2), 3),
                    new Connection((_2, _5), 4),
                    new Connection((_3, _4), 5)
                },
                new int[,] 
                {
                    { 0, 0, 0, 0, 1 },
                    { 0, 0, 1, 0, 1 },
                    { 0, 1, 0, 1, 1 },
                    { 0, 0, 1, 0, 0 },
                    { 1, 1, 1, 0, 0 }
                }
            };
        }
    }
}
