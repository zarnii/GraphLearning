using GraphApp.Model;
using System;
using System.Windows;
using System.Windows.Input;

namespace GraphApp.View
{
    /// <summary>
    /// Окно создания связи.
    /// </summary>
    public partial class CreateConnectionWindow : Window
    {
        /// <summary>
        /// Вес связи по умолчанию.
        /// </summary>
        private int _defaultConnectionWeight = 0;

        /// <summary>
        /// Типы связей.
        /// </summary>
        private ConnectionType[] _connectionsType;

        /// <summary>
        /// Толщина связи по умолчанию.
        /// </summary>
        private int _defaultConnectionThickness = 4;

        /// <summary>
        /// Вес связи.
        /// </summary>
        public int ConnectionWeight { get; private set; }

        /// <summary>
        /// Тип связи.
        /// </summary>
        public ConnectionType ConnectionType { get; private set; }

        /// <summary>
        /// Толщина связи.
        /// </summary>
        public double ConnectionThickness { get; private set; }

        /// <summary>
        /// Окно создания вершины.
        /// </summary>
        public CreateConnectionWindow()
        {
            InitializeComponent();

            Left = (SystemParameters.PrimaryScreenWidth / 2) - (Width / 2);
            Top = (SystemParameters.PrimaryScreenHeight / 2) - (Height / 2);

            _connectionsType = new ConnectionType[]
            {
                ConnectionType.Unidirectional,
                ConnectionType.Bidirectional,
                ConnectionType.NonDirectional
            };

            ConnectionWeightField.Text = _defaultConnectionWeight.ToString();
            ConnectionTypeComboBox.ItemsSource = _connectionsType;
            ConnectionTypeComboBox.SelectedItem = ConnectionType.NonDirectional;
            ConnectionThicknessSlider.Value = _defaultConnectionThickness;
            this.ResizeMode = ResizeMode.NoResize;
        }

        /// <summary>
        /// Проверка вводимого символа на то, что оно является числом.
        /// </summary>
        /// <param name="sender">Инициатор.</param>
        /// <param name="e"></param>
        private void CheckInputSymbolIsDigit(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Соединить.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connect(object sender, RoutedEventArgs e)
        {
            if (ConnectionThicknessSlider.Value < 1)
            {
                throw new ArgumentOutOfRangeException("Толщина связи не может быть меньше 1.");
            }

            ConnectionWeight = int.Parse(ConnectionWeightField.Text);
            ConnectionType = (ConnectionType)ConnectionTypeComboBox.SelectedItem;
            ConnectionThickness = ConnectionThicknessSlider.Value;

            DialogResult = true;
        }
    }
}
