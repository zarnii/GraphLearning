using GraphApp.Services.Converters;

namespace GraphAppTest
{
    [TestClass]
    public class VertexNameCoordinatesConverterTest
    {
        private VertexNameCoordinatesConverter _converter;

        public VertexNameCoordinatesConverterTest()
        {
            _converter = new VertexNameCoordinatesConverter();
        }

        [TestMethod]
        [DataRow(20, 0)]
        [DataRow(int.MaxValue, int.MaxValue - 20)]
        [DataRow(120, 100)]
        [DataRow(317, 297)]
        [DataRow(0, -20)]
        [DataRow(938, 918)]
        public void VertexNameCoordinWtesConverter_Converte_Success(double coordinate, int expected)
        {
            var actual = (int)(double)_converter.Convert(coordinate, null, null, null);

            Assert.AreEqual(expected, actual);
        }
    }
}
