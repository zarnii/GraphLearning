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
    /// Логика взаимодействия для TestGenerateWindow.xaml
    /// </summary>
    public partial class TestGenerateWindow : Window
    {
        /// <summary>
        /// Максимальное количество.
        /// </summary>
        private int _maxCount;

        /// <summary>
        /// Результат пользователя.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="maxCount">Максимально допустимое количество.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public TestGenerateWindow(int maxCount)
        {
            if (maxCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(maxCount), "Максисмальное количество не может быть меньше 0.");
            }

            InitializeComponent();

            Left = (SystemParameters.PrimaryScreenWidth / 2) - (Width / 2);
            Top = (SystemParameters.PrimaryScreenHeight / 2) - (Height / 2);
            _maxCount = maxCount;
        }

        /// <summary>
        /// Фильтрация вводимого текста.
        /// </summary>
        /// <param name="sender">Инициатор события.</param>
        /// <param name="e">Аргументы собатия.</param>
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Нажатие на кнопку "ОК".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            var result = int.Parse(textBox.Text);

            if (result < 1 || result > _maxCount)
            {
                MessageBox.Show(
                     $"Значение должно находиться в диапазоне от 0 до {_maxCount}",
                     "Ошибка",
                     MessageBoxButton.OK,
                     MessageBoxImage.Information
                );

                return;
            }

            Count = result;
            DialogResult = true;
        }

        /// <summary>
        /// Нажатие на кнопку "Cancel".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
