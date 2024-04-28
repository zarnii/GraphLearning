using System.Windows.Media;

namespace GraphAppTest
{
    [TestClass]
    public class ColorConverterTest
    {
        [TestMethod]
        [DynamicData(nameof(GetData), DynamicDataSourceType.Method)]
        public void ColorConverter_Convert_Success(Brush color, Color expectedColor)
        {
            var colorConverter = new GraphApp.Services.Converters.ColorConverter();
            var actualColor = (Color)colorConverter.Convert(color, null, null, null);

            Assert.AreEqual(expectedColor, actualColor);
        }

        private static IEnumerable<object?[]> GetData()
        {
            yield return new object[2] { new SolidColorBrush(Colors.Red), Colors.Red};
            yield return new object[2] { new SolidColorBrush(Colors.Pink), Colors.Pink };
            yield return new object[2] { new SolidColorBrush(Colors.AliceBlue), Colors.AliceBlue };
            yield return new object[2] { new SolidColorBrush(Colors.Aqua), Colors.Aqua };
            yield return new object[2] { new SolidColorBrush(Colors.Black), Colors.Black };
        }
    }
}
