using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace GraphApp.View
{
    /// <summary>
    /// Окно создания вершины.
    /// </summary>
    public partial class CreateVertexWindow : Window
    {
        private int _defautRadius = 10;

        private Color _defaultColor = Colors.Red;

        public string VertexName { get; private set; }

        public int VertexRadius { get; private set; }

        public Color VertexColor { get; private set; }


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
            VertexRadiusSlider.Value = _defautRadius;
            ColorPicker.SelectedColor = _defaultColor;

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
            VertexRadiusSlider.Value = _defautRadius;
            ColorPicker.SelectedColor = _defaultColor;

            ResizeMode = ResizeMode.NoResize;

        }

        private void Create(object sender, RoutedEventArgs e)
        {
            var radius = VertexRadiusSlider.Value;

            if (radius < 1)
            {
                throw new ArgumentOutOfRangeException("Радиус не может меньше 1.");
            }

            VertexRadius = (int)radius;
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

        private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> e)
        {
            if (e.NewValue == null)
            {
                return;
            }

            VertexColor = e.NewValue.Value; 
        }
    }
}
