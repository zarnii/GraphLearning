using GraphApp.Model;
using System.Windows.Media;

namespace GraphAppTest
{
    [TestClass]
    public class VertexTest
    {

        [TestMethod]
        [DataRow(10, 10, 10,   10, "MyVertex")]
        [DataRow(21, 12, 11,   -5, "--")]
        [DataRow(30, 20, 71,  100, "")]
        [DataRow(35, 13,  5,  -11, "21312")]
        [DataRow(-4, 16, 80,  320, "SecondVertex")]
        public void VisualVertex_Create_Success(double x, double y, int radius, int number, string name)
        {
            var vertex = new VisualVertex(x, y, radius, number, Colors.Red, name);

            Assert.AreEqual(x, vertex.X); 
            Assert.AreEqual(y, vertex.Y); 
            Assert.AreEqual(radius, vertex.Radius);
            Assert.AreEqual(number, vertex.Number);
            Assert.AreEqual(name, vertex.Name);
        }

        [TestMethod]
        [DataRow(10, 10,  1, 10, "MyVertex")]
        [DataRow(21, 12, -1, -5, "--")]
        [DataRow(30, 20,  2, 100, "")]
        [DataRow(35, 13, -5, -11, "21312")]
        [DataRow(-4, 16,  0, 320, "SecondVertex")]
        public void VisualVertex_Create_RadiusOutOfRange(double x, double y, int radius, int number, string name)
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                new VisualVertex(x, y, radius, number, Colors.Red, name);
            });
        }
    }
}