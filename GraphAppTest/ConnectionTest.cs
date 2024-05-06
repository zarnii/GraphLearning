using GraphApp.Model;
using System.Windows.Media;

namespace GraphAppTest
{
    [TestClass]
    public class ConnectionTest
    {
        [TestMethod]
        [DynamicData(nameof(GenerateData), DynamicDataSourceType.Method)]
        public void VisualConnection_Create_Success(
            VisualVertex firstVertex, 
            VisualVertex secondVertex,
            int thickness,
            int number, 
            int weight, 
            ConnectionType connectionType)
        {
            var connection = new VisualConnection(
                (firstVertex, secondVertex), number, thickness, weight, connectionType);

            Assert.AreEqual(firstVertex, connection.ConnectedVertices.Item1);
            Assert.AreEqual(secondVertex, connection.ConnectedVertices.Item2);
            Assert.AreEqual(number, connection.Number);
            Assert.AreEqual(thickness, connection.Thickness);
            Assert.AreEqual(weight, connection.Weight);
            Assert.AreEqual(connectionType, connection.ConnectionType);
        }

        [TestMethod]
        [DataRow(1,   0,  -1, ConnectionType.Bidirectional)]
        [DataRow(1, -11,  22, ConnectionType.Bidirectional)]
        [DataRow(1, -32, 101, ConnectionType.Bidirectional)]
        [DataRow(1,  -1,   1, ConnectionType.Bidirectional)]
        public void VisualConnection_Create_ArgumentOutOfRange(
            int number, 
            int thickness, 
            int weight, 
            ConnectionType connectionType)
        {
            var first = new VisualVertex(10, 10, 10, 10, Colors.Red, "First");
            var second = new VisualVertex(10, 10, 10, 10, Colors.Red, "Second");

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                new VisualConnection((first, second), number, thickness, weight, connectionType);
            });
        }

        private static IEnumerable<object[]?> GenerateData()
        {
            yield return new object[]
            {
                new VisualVertex(10, 10, 10, 10, Colors.Red, "First"),
                new VisualVertex(10, 10, 10, 10, Colors.Red, "Second"),
                5,
                1,
                10,
                ConnectionType.Bidirectional
            };
            yield return new object[]
            {
                new VisualVertex(10, 10, 10, 10, Colors.Red, "First"),
                new VisualVertex(10, 10, 10, 10, Colors.Red, "Second"),
                85,
                10,
                1,
                ConnectionType.NonDirectional
            };
            yield return new object[]
            {
                new VisualVertex(10, 10, 10, 10, Colors.Red, "First"),
                new VisualVertex(10, 10, 10, 10, Colors.Red, "Second"),
                12,
                13,
                -1,
                ConnectionType.Unidirectional
            };
            yield return new object[]
            {
                new VisualVertex(10, 10, 10, 10, Colors.Red, "First"),
                new VisualVertex(10, 10, 10, 10, Colors.Red, "Second"),
                2222,
                23,
                10100,
                ConnectionType.Bidirectional
            };
        }
    }
}
