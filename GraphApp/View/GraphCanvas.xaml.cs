using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphApp.View
{
    /// <summary>
    /// Логика взаимодействия для GraphCanvas.xaml
    /// </summary>
    public partial class GraphCanvas : UserControl
    {
        private const int GridWidth = 1000;

        private const int GridHeight = 900;

        private const int ScaleXLimit = 5;

        private const int ScaleYLimit = 5;

        public GraphCanvas()
        {
            InitializeComponent();
            MainGrid.Width = GridWidth;
            MainGrid.Height = GridHeight;
            
        }

        private void MainGrid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            
            double zoom = e.Delta > 0
                ? 0.1
                : -0.1;


            if (scaleTransformGrid.ScaleX + zoom < 0.1 || scaleTransformGrid.ScaleY + zoom < 0.1)
            {
                return;
            }

            if (scaleTransformGrid.ScaleX + zoom > ScaleXLimit || scaleTransformGrid.ScaleY + zoom > ScaleYLimit)
            {
                return;
            }

            scaleTransformGrid.ScaleX += zoom;
            scaleTransformGrid.ScaleY += zoom;
        }
    }
}
