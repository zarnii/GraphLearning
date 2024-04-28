using GraphApp.Model;
using GraphApp.Services.Converters;
using System.Windows.Media;

namespace GraphAppTest
{
    [TestClass]
    public class ConnectionWeightCoordinatesConverterTest
    {
        private readonly ConnectionWeightCoordinatesConverter _converter;
        
        public ConnectionWeightCoordinatesConverterTest() 
        { 
            _converter = new ConnectionWeightCoordinatesConverter();
        }

        [TestMethod]
        [DynamicData(nameof(GenerateData), DynamicDataSourceType.Method)]
        public void ConnectionWeightCoordinatesConverter_Convert_Success(
            double coord1, 
            double coord2, 
            VisualVertex firstVertex, 
            VisualVertex secondVetex,
            double expected)
        {
            var actual = (double)_converter.Convert(new object[4] { coord1, coord2, firstVertex, secondVetex }, null, null, null);
            
            Assert.AreEqual(expected, actual);
        }

        private static IEnumerable<object?[]> GenerateData()
        {


            yield return new object[5]
            {
                10,
                10, 
                new VisualVertex(10, 10, 20, 1, Colors.Red),
                new VisualVertex(10, 10, 20, 2, Colors.Red),
                10
            };
            yield return new object[5]
            {
                212,
                321,
                new VisualVertex(10, 10, 20, 1, Colors.Red),
                new VisualVertex(10, 10, 20, 2, Colors.Red),
                266.5
            };
            yield return new object[5]
            {
                45,
                10,
                new VisualVertex(10, 10, 20, 1, Colors.Red),
                new VisualVertex(10, 10, 20, 1, Colors.Red),
                27.5
            };
            yield return new object[5]
            {
                754,
                555,
                new VisualVertex(10, 10, 20, 1, Colors.Red),
                new VisualVertex(10, 10, 20, 1, Colors.Red),
                654.5
            };
        }
    }
}
