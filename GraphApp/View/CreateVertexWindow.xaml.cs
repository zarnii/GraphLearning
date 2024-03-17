using DocumentFormat.OpenXml.Drawing.Charts;
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
using System.Windows.Shapes;

namespace GraphApp.View
{
    /// <summary>
    /// Логика взаимодействия для CreateVertexWindow.xaml
    /// </summary>
    public partial class CreateVertexWindow : Window
    {
        private int _defautRadius = 10;

        public string VertexName { get; private set; }

        public int VertexRadius { get; private set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="left">Позиция левого края окна относительно рабочего стола.</param>
        /// <param name="top">Позиция верхнего края окна относительно рабочего стола.</param>
        public CreateVertexWindow(double left, double top)
        {
            if (left < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(left), "Позиция окна не может быть меньше 0.");
            }

            if (top < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(left), "Позиция окна не может быть меньше 0.");
            }

            InitializeComponent();

            Left = left;
            Top = top;

            VertexNameField.Text = "default";
            VertexRadiusField.Text = _defautRadius.ToString();

            ResizeMode = ResizeMode.NoResize;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public CreateVertexWindow()
        {
            InitializeComponent();

            Left = (SystemParameters.PrimaryScreenWidth / 2) - (Width / 2);
            Top = (SystemParameters.PrimaryScreenHeight / 2) - (Height / 2);

            VertexNameField.Text = "default";
            VertexRadiusField.Text = _defautRadius.ToString();

            ResizeMode = ResizeMode.NoResize;

        }
        
        private void Create(object sender, RoutedEventArgs e)
        {
            var radius = int.Parse(VertexRadiusField.Text);

            if (radius < 1)
            {
                throw new ArgumentOutOfRangeException("Радиус не может меньше 1.");
            }

            VertexRadius = radius;
            VertexName = VertexNameField.Text;

            DialogResult = true;
        }

        private void CheckInputSymbolIsDigit(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }
    }
}
