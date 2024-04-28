using GraphApp.Services.Converters;

namespace GraphAppTest
{
    [TestClass]
    public class ConnectionCoordinatesConverterTest
    {
        private ConnectionCoordinatesConverter _converter;

        public ConnectionCoordinatesConverterTest() 
        { 
            _converter = new ConnectionCoordinatesConverter();
        }

        [TestMethod]
        [DataRow(10, 123, 45, 81, 10, "X", 38)]
        [DataRow(10, 123, 45, 81, 10, "Y", 88)]
        [DataRow(300, 1, 23, 76, 20, "X", 42)]
        [DataRow(300, 1, 23, 76, 20, "Y", 70)]
        [DataRow(60, 12, 12, 34, 30, "X", 39)]
        [DataRow(60, 12, 12, 34, 30, "Y", 21)]
        [DataRow(450, 0, 23, 43, 5, "X", 27)]
        [DataRow(450, 0, 23, 43, 5, "Y", 42)]
        public void ConnectionCoordinatesConverter_Convert_Success(
            double x1, 
            double y1, 
            double x2, 
            double y2, 
            int radius,
            string coord,
            int expected)
        {
            var actual = (int)(double)_converter.Convert(new object[5] { x1, y1, x2, y2, radius }, null, coord, null);

            Assert.AreEqual(expected, actual);
        }
    }
}
